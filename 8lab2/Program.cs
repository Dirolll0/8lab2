using System;

class Koord
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

abstract class Ship
{
    public int ShipNumber { get; set; }
    public Koord Coordinates { get; set; }

    public abstract void InputData();

    public void DisplayCoordinates()
    {
        Console.WriteLine($"Корабль {ShipNumber} координаты: Широта {Coordinates.Latitude} С, Долгота {Coordinates.Longitude} В");
    }

    public abstract double CalculateDistance(Ship otherShip);
}

class MyShip : Ship
{
    public MyShip(int shipNumber)
    {
        ShipNumber = shipNumber;
        Coordinates = new Koord();
    }

    public override void InputData()
    {
        Console.WriteLine($"Введите координаты корабля {ShipNumber}:");

        bool validInput = false;

        do
        {
            try
            {
                Console.Write("Широта: ");
                Coordinates.Latitude = Convert.ToDouble(Console.ReadLine());

                if (Coordinates.Latitude < -90 || Coordinates.Latitude > 90)
                {
                    throw new ArgumentException("Широта должна быть в диапазоне от -90 до 90.");
                }

                Console.Write("Долгота: ");
                Coordinates.Longitude = Convert.ToDouble(Console.ReadLine());

                if (Coordinates.Longitude < -180 || Coordinates.Longitude > 180)
                {
                    throw new ArgumentException("Долгота должна быть в диапазоне от -180 до 180.");
                }

                validInput = true;
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Некорректный формат данных. Пожалуйста, введите числовое значение.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        } while (!validInput);
    }

    public override double CalculateDistance(Ship otherShip)
    {
        double lat1 = Coordinates.Latitude;
        double lon1 = Coordinates.Longitude;
        double lat2 = otherShip.Coordinates.Latitude;
        double lon2 = otherShip.Coordinates.Longitude;

        double R = 6371; 

        double lat1Rad = ToRadians(lat1);
        double lon1Rad = ToRadians(lon1);
        double lat2Rad = ToRadians(lat2);
        double lon2Rad = ToRadians(lon2);

        double dLat = lat2Rad - lat1Rad;
        double dLon = lon2Rad - lon1Rad;

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        double distance = R * c;

        return distance;
    }

    private static double ToRadians(double angle)
    {
        return Math.PI * angle / 180.0;
    }
}

class Program
{
    static void Main()
    {
        MyShip ship1 = new MyShip(1);
        MyShip ship2 = new MyShip(2);

        ship1.InputData();
        ship2.InputData();

        ship1.DisplayCoordinates();
        ship2.DisplayCoordinates();

        double distance = ship1.CalculateDistance(ship2);
        Console.WriteLine($"Расстояние между кораблем 1 и кораблем 2: {distance} км");
    }
}