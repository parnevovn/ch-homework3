## Задание:

1. Разделить решение на 3 проекта (Бизнесс логика, Инфраструктура, Приложение)
   1. Реализации сервисов в бизнесс логике и в инфраструктуре должны быть `internal`
   2. Регистрацию в DI для проекта бизнесс логики может быть реализована как в проекте с бизнесс логикой так и в проекте с инфраструктурой
2. Сделать рефакторинг класса `GoodPriceCalculatorService` чтобы он соответствовал принятым конвенциям
3. Написать юнит-тесты для класса `GoodPriceCalculatorService`
4. 💎 Дополнительное задание
   1. Cодать проект c интеграционными тестами. 
   2. Написать 2 простых интеграционных теста для маршрута `/V3DeliveryPrice/good/calculate`