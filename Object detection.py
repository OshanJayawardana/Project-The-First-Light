import cv2
thres = 0.65     # Threshold to detect object
num_frame=100     # Algorithm running itteration count
cap = cv2.VideoCapture(0)
cap.set(3,1280)
cap.set(4,720)
cap.set(10,70)

classNames= []
classFile = 'coco.names'

#get element frequency

def freq(lis, num):
    frequency = {}
    for item in lis:
        if item in frequency:
            frequency[item] += 1
        else:
            frequency[item] = 1
    elem = list(frequency)
    out = []
    count = 0
    for i in frequency.values():
        if i >= int(num):
            out.append(elem[count])
        count += 1

    return out

with open(classFile,'rt') as f:
    classNames = f.read().rstrip('\n').split('\n')

configPath = 'ssd_mobilenet_v3_large_coco_2020_01_14.pbtxt'
weightsPath = 'frozen_inference_graph.pb'

net = cv2.dnn_DetectionModel(weightsPath,configPath)
net.setInputSize(320,320)
net.setInputScale(1.0/ 127.5)
net.setInputMean((127.5, 127.5, 127.5))
net.setInputSwapRB(True)
a=0
l=[]
while True:
    a+=1
    success, img = cap.read()
    classIds, confs, bbox = net.detect(img,confThreshold=thres)
    #qprint(classIds,bbox)

    if len(classIds) != 0:
        for classId, confidence, box in zip(classIds.flatten(), confs.flatten(), bbox):
            cv2.rectangle(img, box, color=(0, 255, 0), thickness=2)
            cv2.putText(img, classNames[classId - 1].upper(), (box[0] + 10, box[1] + 30),
                        cv2.FONT_HERSHEY_COMPLEX, 1, (0, 255, 0), 2)
            l.append(classNames[classId - 1].upper())
            cv2.putText(img, str(round(confidence * 100, 2)), (box[0] + 200, box[1] + 30),
                        cv2.FONT_HERSHEY_COMPLEX, 1, (0, 255, 0), 2)

    cv2.imshow("Output",img)
    if a<=num_frame:
        cv2.waitKey(1)
    else:
        while a>0:
            if cv2.waitKey(1) == ord(' '):
                a=0
                l = freq(l, num_frame / 2)  # discard if element count is below than given number
                if len(l) == 0 :
                    l.append("Object not Detected")
                #l = list(dict.fromkeys(l))
                print(l)
                l=[]