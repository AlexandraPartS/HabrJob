# Проект "Парсер Habr для поиска и сбора данных по вакансиям по заданным параметрам с экспортом в XML"

Пользователь на career.habr.com вводит необходимые параметры поиска. Полученную ссылку вводит в приложение и 
получает все релевантные вакансии с данными в файле XML. 
Используются библиотеки HtmlAgilityPack, Newtonsoft.Json.

### Пользовательский сценарий работы:
1. Пользователь вводит ссылку на страницу career.habr.com с параметрами поиска по типу: ```https://career.habr.com/vacancies?q=c%23&qid=4&type=all```
    * интерактив с пользователем происходити через консоль.
    * ссылка проверяется на валидность пока не будет получен валидный результат.
2. При не нулевых результатах поиска:
    * на консоль выводится информация по количеству найденных результатов и директива на XML файл.
    * по указанному адресу создается XML файл с собранными данными по результатам поиска.
3. При нулевых результатах поиска:
    * на консоль выводится соответствующая информация.
    * XML файл не создается.

В работе используются следующие классы: DataReceiver, PageManager, SearchResult, HabrJob, Parser, DataExport, Program.




