.386
.MODEL FLAT, C

.DATA
a DD ?

.CODE
MAIN PROC

MOV EBX, 10
MOV a, EBX

MOV EAX, 1
MOV EBX, a
ADD EAX, EBX
MOV a, EAX

MAIN ENDP
END MAIN