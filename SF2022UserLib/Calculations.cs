using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF2022UserLib
{
    public class Calculations
    {
        public string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
        {
            List<string> availablePeriods = new List<string>();

            // Преобразуем занятые промежутки в список интервалов
            List<(TimeSpan Start, TimeSpan End)> busyPeriods = new List<(TimeSpan, TimeSpan)>();
            for (int i = 0; i < startTimes.Length; i++)
            {
                busyPeriods.Add((startTimes[i], startTimes[i].Add(TimeSpan.FromMinutes(durations[i]))));
            }

            // Сортируем занятые промежутки по времени начала
            busyPeriods.Sort((a, b) => a.Start.CompareTo(b.Start));

            // Начинаем с начала рабочего дня
            TimeSpan currentTime = beginWorkingTime;

            // Перебираем занятые промежутки
            foreach (var busyPeriod in busyPeriods)
            {
                // Если текущее время меньше начала занятого промежутка, добавляем свободные интервалы
                if (currentTime < busyPeriod.Start)
                {
                    AddAvailablePeriods(currentTime, busyPeriod.Start, consultationTime, availablePeriods);
                }

                // Перемещаем текущее время на конец занятого промежутка
                currentTime = busyPeriod.End;

                // Если текущее время вышло за пределы рабочего дня, прерываем цикл
                if (currentTime >= endWorkingTime)
                    break;
            }

            // Добавляем оставшиеся свободные интервалы до конца рабочего дня
            if (currentTime < endWorkingTime)
            {
                AddAvailablePeriods(currentTime, endWorkingTime, consultationTime, availablePeriods);
            }

            return availablePeriods.ToArray();
        }

        private void AddAvailablePeriods(TimeSpan start, TimeSpan end, int consultationTime, List<string> availablePeriods)
        {
            TimeSpan current = start;

            while (current.Add(TimeSpan.FromMinutes(consultationTime)) <= end)
            {
                availablePeriods.Add($"{current:hh\\:mm}-{current.Add(TimeSpan.FromMinutes(consultationTime)):hh\\:mm}");
                current = current.Add(TimeSpan.FromMinutes(consultationTime));
            }
        }

    }
}
