Ресурс:"api/verifylicense/test/{id}"
Пример:http://{base_url}/api/verifylicense/test/E1BA42A3-C5CA-4700-B7F1-D2FF8C361F6E
HTTP Method: POST
Request:
{
}
Responce:
{  
   "Id":"e1ba42a3-c5ca-4700-b7f1-d2ff8c361f6e", //идентификатор на лиценза, уникален ключ за всеки
   "ValidTo":"2017-10-11T20:30:26.617",//докога е валиден, това се изпраща при създаване на лиценза, като за демо ще бъде ограничен до месец
   "SubscribedTo":"2017-10-11T20:30:26.617", //докога е абониран за ъпдейти и подръжка
   "IsDemo":true,
   "User":{  //потребителя, за/който е закупил лиценза
      "PostCode":0,//пощенски код
      "RegistrationAddress":null,//адрес на регистрация
      "PostAddress":null,//пощенски адрес
      "MOL":null,//МОЛ
      "ContactPerson":null,//Лице за контакт
      "AccountingPerson":null,//Счетоводител
      "DDSRegistration":false,//има ли регистрация по ДДС
      "Name":"test123",//име на лицето закупил лиценза/може и фирма
      "IsCompany":true,//дали е фирма
      "Phone":"123",//телефон
      "Email":"111@test",//електрона поща
      "CompanyId":null,//БУЛСТАТ ако е фирма
      "EGN":null//ЕГН ако не е фирма
   },
   "Modules"://закупени модули, не е задължително да са всички
   [  
	   1, //Счетоводство
	   2, //Производство
	   3, //Склад
	   4, //Търговска система
	   5, //ТРЗ
	   6  //Графици
   ],
   "Type":1//Вид лиценз, възможни стойности са три: 1 - За компютър, 2 - За потребител, 3 - За сървър
}

------------------------------------------------------------------------------------------------------------------------------
Ресурс:"api/verifylicense/{id}"
Пример:http://{base_url}/api/verifylicense/E1BA42A3-C5CA-4700-B7F1-D2FF8C361F6E
HTTP Method: POST
Request:
{
}
Responce:
{  
    "Key": "m/vR3Ifw7I/043+BwMD6u7Iur94dXN1hvFj/wvGjZ5JRd+8zlxPEpOHHpHxQ+no6vyzZh59iGFpGP/YDfv55bN7zRrpR971zgmQRlwa2kUc=", //да се декриптира със ключ a4ffaf8ac92e4ffcb8edbdcb459bcef1
    "License": "9XrGJAQFqKMoa+8DSEMiGsVVtYoAvxPXbVUk3wvAwpVaeWKi00Z9IhcrTFO6KEaBdchie3p5WS2A9d94epYumEB44yb7YuNgEzmb36FA1P1452mFhPJynwxf7ub6icmkXuaHhaFGpsiu0kmMCoKVUk0IDaPLELDZ0kwJTlaDENR2h5S/x9bYdQDJY1CBhvDfSGuioHU1Tiw/UpuDMM9RBLBOlWwFWaEiE/g0rPLPqW4aky8yE346pihhMiJqZDq2skM1wGfTsNm5yYBkX0InvpQBaNt55iw43gSZHgxciUoqpehuArw+e/Ryf0AJe2pkR7OYFkWSrHbw75Eb655XkiAsQSq31OhJ+l+LbATimZ+f8+b04ih+sGbQ92ej+BuaIWUcPSVworOvTI6BqKrh+AsHkMHfQmHVQMXdt7+EyQoOdmQtcBBAjEllijJZIIaIuhdaK50f02ZkgUloqG3TmqufolW7b7AEeBQHwMYNY/g=",//да се декриптира с горния ключ
    "LicenseId": "E1BA42A3-C5CA-4700-B7F1-D2FF8C361F6E"//за проверка
}

Mетод на криптиране = AES-256
Mode = CBC
PaddingMode = PKCS7
SaltSize = 32
