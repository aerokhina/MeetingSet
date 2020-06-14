Для запуска программы необходимо:
1. Установить .NET Core SDK 3.1
2. Установить PostgreSQL v12 (при установке задать имя пользователя и пароль как в конфигурации, по умолчанию User Id=postgres; Password=12345)
3. Выполнить команду dotnet publish -c release
4. Запустить программу по адресу MeetingSet\MeetingSet\bin\Release\netcoreapp3.1\publish\MeetingSet.exe

Всем участникам митинга отправляются уведомления по почте за 15 минут до начала митинга.
Этот процесс настраивается в файле appsettings.json в разделе "MeetingNotification". 
- В "MeetingDelayMinutes" указывается количество минут до начала митинга для отправки уведомления.
- В "PollingDelaySeconds" указывается частота опроса БД в секундах.

Для всех email уведомлений есть следующие настройки в файле appsettings.json в разделе "Email"
- В "From" указывается email отправителя(с какой почты будут отсылаться письма)
- В "Host" указывается адрес smpt сервера
- В "Port" указывается порт smpt сервера
- В "UserName" указывается логин от почтового аккаунта
- В "Password" указывается пароль от почтового аккаунта


Существуют следующие запросы:
- Создать встречу:
POST http://localhost:5000/meeting/create 
body:
{
  "name": "testMeeting1",
  "startDateTimeMeeting": "2007-04-05T18:05",
  "endDateTimeMeeting": "2007-04-05T18:35"
}
response:
{
    "id": 1,
    "name": "testMeeting1",
    "startDateTimeMeeting": "2007-04-05T18:05:00",
    "endDateTimeMeeting": "2007-04-05T18:35:00"
}

-Создать участника:
POST http://localhost:5000/participant/create
body:
{
  "name": "ElsaSa",
  "email": "ed@.ru"
}
Email проверяется на валидность
response:
{
    "id": 1,
    "name": "ElsaSa",
    "email": "ed@ru"
}

-Удалить встречу
POST http://localhost:5000/meeting/delete/1

-Удалить участника:
POST  http://localhost:5000/participant/delete/1

-Добавить участника на встречу:
POST  http://localhost:5000/meeting/1/addParticipant/1
Проверка занят/свободен ли участник для встречи.
Если участник свободен, он добавляется на встречу, и ему на почту приходит уведомление о том, что он был добавлен на митинг.

-Удалить участника из встречи:
POST  http://localhost:5000/meeting/1/removeParticipant/1

-Получить список встреч с участниками:
POST http://localhost:5000/meeting/getList
response:
[
    {
        "id": 9,
        "name": "testMeeting1",
        "startDateTimeMeeting": "2007-04-05T19:20:00",
        "endDateTimeMeeting": "2007-04-05T19:30:00",
        "participants": [
            {
                "id": 6,
                "name": "ElsaSa",
                "email": "ed@w"
            }
        ]
    },
    {
        "id": 10,
        "name": "testMeeting1",
        "startDateTimeMeeting": "2007-04-05T18:00:00",
        "endDateTimeMeeting": "2007-04-05T18:10:00",
        "participants": [
            {
                "id": 5,
                "name": "ElsaSa",
                "email": "ed@w"
            }
        ]
    },
]

