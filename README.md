# Lab Service
____
## Вступление
Проект представляет собой 8 проектов, связанных между друг другом. Двумя главными проектами являются Windows-службы(DataManager и FileManager).
FileManager и конфигурацию к нему(проект ConfigurationManager) мы разработали в прошлых лабораторных работах. Теперь настало время получения данных из базы данных
и записи событий и исключений службы в отдельную базу данных. Для этого разработаем службу DataManager, слой DataAccess для работы с базой данных, слой ServiceLayer,
в котором будет описана логика работы с данными полученными из базы данных, а также XmlGenerator, который будет генерировать xml файл и xsd схему на основе полученных данных.
Также дополнительно разработаем слой для работы с базой данных для логгирования.
## DataAccess
____
DataAccess содержит в себе класс ShippingContext, посредством которого заполняются репозитории заказов OrdersRepository.

*Метод ReadData ShippingContext*
```C#
     public void ReadData()      //Считывание данных из базы данных
     {
          using (SqlConnection connection = new SqlConnection(ConnectionString))
          {
              connection.Open();
              SqlCommand command = new SqlCommand(StoredProcedure, connection)
              {
                  CommandType = CommandType.StoredProcedure
              };
              SqlDataAdapter adapter = new SqlDataAdapter(command);
              adapter.Fill(OrdersSet);
          }
     }
```
Метод взаимодействует с базой с помощью хранимой процедуры
![Alt-text]("Screenshots/Screenshot 2020-12-09 230659".jpg "Хранимая процедура")
Таблица в базе данных
![Alt-text](Screenshots/zDszA1CsU7k.jpg "Таблица")
Далее данные конвертируются в IEnumerable и помещаются в репозиторий. ShippingContext использует модель Order из проекта Models папки DataBaseModels.
Репозиторий находится в UnitOfWork посредством которого ServiceLayer взаимодействует с DataAccess.
## ServiceLayer
ServiceLayer содержит в себе логику преобразования Order в OrderDTO(Data transfer object), он преобразует несколько заказов один, вычисляя общую
стоимость заказа, и собирая все имена продуктов в один список, получая единый объект заказа OrderDTO.

*Метод перевода нескольких заказов в один*
```C#
        private OrderDTO ConvertOrderToOrderDTO(IEnumerable<Order> orders)  
        {
            if (orders == null)
            {
                throw new Exception("Заказ не найден. Ошибка Service Layer.");
            }
            if (orders.Contains(null))
            {
                throw new Exception("Одно из полей заказа не установлено. Ошибка Service Layer.");
            }
            decimal totalPrice = 0;
            List<string> names = new List<string>();
            foreach (Order order in orders)
            {
                totalPrice += order.UnitPrice * order.Quantity;
                names.Add(order.ProductName);
            }
            Order firstOrder = orders.First();
            return new OrderDTO()
            {
                Address = firstOrder.Address,
                City = firstOrder.City,
                CompanyName = firstOrder.CompanyName,
                ContactName = firstOrder.ContactName,
                Country = firstOrder.Country,
                Id = firstOrder.Id,
                OrderPrice = totalPrice,
                ProductNames = names,
                RequiredDate = firstOrder.RequiredDate
            };
        }
```
