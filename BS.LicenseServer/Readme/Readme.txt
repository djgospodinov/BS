������:"api/verifylicense/test/{id}"
������:http://{base_url}/api/verifylicense/test/E1BA42A3-C5CA-4700-B7F1-D2FF8C361F6E
HTTP Method: POST
Request:
{
}
Responce:
{  
   "Id":"e1ba42a3-c5ca-4700-b7f1-d2ff8c361f6e", //������������� �� �������, �������� ���� �� �����
   "ValidTo":"2017-10-11T20:30:26.617",//������ � �������, ���� �� ������� ��� ��������� �� �������, ���� �� ���� �� ���� ��������� �� �����
   "SubscribedTo":"2017-10-11T20:30:26.617", //������ � �������� �� ������� � ��������
   "IsDemo":true,
   "User":{  //�����������, ��/����� � ������� �������
      "PostCode":0,//�������� ���
      "RegistrationAddress":null,//����� �� �����������
      "PostAddress":null,//�������� �����
      "MOL":null,//���
      "ContactPerson":null,//���� �� �������
      "AccountingPerson":null,//������������
      "DDSRegistration":false,//��� �� ����������� �� ���
      "Name":"test123",//��� �� ������ ������� �������/���� � �����
      "IsCompany":true,//���� � �����
      "Phone":"123",//�������
      "Email":"111@test",//��������� ����
      "CompanyId":null,//������� ��� � �����
      "EGN":null//��� ��� �� � �����
   },
   "Modules"://�������� ������, �� � ������������ �� �� ������
   [  
	   1, //������������
	   2, //������������
	   3, //�����
	   4, //��������� �������
	   5, //���
	   6  //�������
   ],
   "Type":1//��� ������, �������� ��������� �� ���: 1 - �� ��������, 2 - �� ����������, 3 - �� ������
}

------------------------------------------------------------------------------------------------------------------------------
������:"api/verifylicense/{id}"
������:http://{base_url}/api/verifylicense/E1BA42A3-C5CA-4700-B7F1-D2FF8C361F6E
HTTP Method: POST
Request:
{
}
Responce:
{  
    "Key": "m/vR3Ifw7I/043+BwMD6u7Iur94dXN1hvFj/wvGjZ5JRd+8zlxPEpOHHpHxQ+no6vyzZh59iGFpGP/YDfv55bN7zRrpR971zgmQRlwa2kUc=", //�� �� ���������� ��� ���� a4ffaf8ac92e4ffcb8edbdcb459bcef1
    "License": "9XrGJAQFqKMoa+8DSEMiGsVVtYoAvxPXbVUk3wvAwpVaeWKi00Z9IhcrTFO6KEaBdchie3p5WS2A9d94epYumEB44yb7YuNgEzmb36FA1P1452mFhPJynwxf7ub6icmkXuaHhaFGpsiu0kmMCoKVUk0IDaPLELDZ0kwJTlaDENR2h5S/x9bYdQDJY1CBhvDfSGuioHU1Tiw/UpuDMM9RBLBOlWwFWaEiE/g0rPLPqW4aky8yE346pihhMiJqZDq2skM1wGfTsNm5yYBkX0InvpQBaNt55iw43gSZHgxciUoqpehuArw+e/Ryf0AJe2pkR7OYFkWSrHbw75Eb655XkiAsQSq31OhJ+l+LbATimZ+f8+b04ih+sGbQ92ej+BuaIWUcPSVworOvTI6BqKrh+AsHkMHfQmHVQMXdt7+EyQoOdmQtcBBAjEllijJZIIaIuhdaK50f02ZkgUloqG3TmqufolW7b7AEeBQHwMYNY/g=",//�� �� ���������� � ������ ����
    "LicenseId": "E1BA42A3-C5CA-4700-B7F1-D2FF8C361F6E"//�� ��������
}

M���� �� ���������� = AES-256
Mode = CBC
PaddingMode = PKCS7
SaltSize = 32
