.386
.MODEL FLAT, C

.DATA
a DD ?

.CODE
MAIN:

MOV EBX, 10
MOV a, EBX

MOV EAX, 1
MOV EBX, a
ADD EAX, EBX
MOV a, EAX

END MAIN