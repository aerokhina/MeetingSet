Для запуска программы необходимо:
1. Установить .NET Core SDK 3.1
2. Установить PostgreSQL v12 (при установке задать имя пользователя и пароль как в конфигурации, по умолчанию User Id=postgres; Password=12345)
3. Выполнить команду dotnet publish -c release
4. Запустить программу по адресу MeetingSet\MeetingSet\bin\Release\netcoreapp3.1\publish\MeetingSet.exe

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

-Удалить участника из встречи:
POST  http://localhost:5000/meeting/1/removeParticipant/1

-Получить список встреч с участниками:
POST http://localhost:5000/meeting/getList
responce:
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