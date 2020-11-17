using System.Text.Json;

namespace Lab2_service
{
    public class JsonParser : Parser
    {
        protected override string GetProperty(string name)
        {
            JsonElement root = JsonDocument.Parse(documentText).RootElement; //Получим корневой элемент json документа
            string property = root.GetProperty(name).GetRawText().Trim('"'); //Найдем свойство по имени в формате строки
                                                                             //В случае ненайденного свойства будет вызвана исключительная ситуация
            return property;                                                 //Вернем свойство
        }
    }
}
