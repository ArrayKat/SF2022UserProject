using SF2022UserLib;

namespace SF2022UserTest
{
    [TestClass]
    public sealed class Test1
    {
        /// <summary>
        /// Нет занятых промежутков
        /// </summary>
        [TestMethod]
        public void AvailablePeriods_NoBusyIntervals_ReturnsFullDay()
        {

            // Arrange
            var startTimes = new TimeSpan[] { };
            var durations = new int[] { };
            var beginWorkingTime = new TimeSpan(8, 0, 0);
            var endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            var calculations = new Calculations();

            // Act
            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // Assert
            Assert.AreEqual(20, result.Length); // 10 часов * 2 интервала в час
            CollectionAssert.Contains(result, "08:00-08:30");
            CollectionAssert.Contains(result, "17:30-18:00");
        }

        /// <summary>
        /// Занятый промежуток в начале рабочего дня
        /// </summary>
        [TestMethod]
        public void AvailablePeriods_BusyIntervalAtStart_ReturnsIntervalsAfterBusyTime()
        {
            // Arrange
            var startTimes = new TimeSpan[] { new TimeSpan(8, 0, 0) };
            var durations = new int[] { 60 };
            var beginWorkingTime = new TimeSpan(8, 0, 0);
            var endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            var calculations = new Calculations();

            // Act
            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // Assert
            CollectionAssert.Contains(result, "09:00-09:30");
            CollectionAssert.DoesNotContain(result, "08:00-08:30");
        }
        /// <summary>
        /// Занятый промежуток в конце рабочего дня
        /// </summary>
        [TestMethod]
        public void AvailablePeriods_BusyIntervalAtEnd_ReturnsIntervalsBeforeBusyTime()
        {
            // Arrange
            var startTimes = new TimeSpan[] { new TimeSpan(17, 0, 0) };
            var durations = new int[] { 60 };
            var beginWorkingTime = new TimeSpan(8, 0, 0);
            var endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            var calculations = new Calculations();

            // Act
            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // Assert
            CollectionAssert.Contains(result, "16:30-17:00");
            CollectionAssert.DoesNotContain(result, "17:30-18:00");
        }
        /// <summary>
        /// Недостаточно времени для консультации
        /// </summary>
        [TestMethod]
        public void AvailablePeriods_NotEnoughTimeBetweenIntervals_ReturnsValidIntervals()
        {
            // Arrange
            var startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0), new TimeSpan(10, 45, 0) };
            var durations = new int[] { 30, 30 };
            var beginWorkingTime = new TimeSpan(8, 0, 0);
            var endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            var calculations = new Calculations();

            // Act
            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // Assert
            CollectionAssert.DoesNotContain(result, "10:30-11:00");
        }
        /// <summary>
        /// Консультация длительностью 1 час
        /// </summary>
        [TestMethod]
        public void AvailablePeriods_ConsultationTime60Minutes_ReturnsValidIntervals()
        {
            // Arrange
            var startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0) };
            var durations = new int[] { 60 };
            var beginWorkingTime = new TimeSpan(8, 0, 0);
            var endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 60;

            var calculations = new Calculations();

            // Act
            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // Assert
            CollectionAssert.Contains(result, "08:00-09:00");
            CollectionAssert.Contains(result, "11:00-12:00");
        }
        /// <summary>
        /// Занятые промежутки перекрываются
        /// </summary>
        [TestMethod]
        public void AvailablePeriods_OverlappingIntervals_ReturnsValidIntervals()
        {
            // Arrange
            var startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0), new TimeSpan(10, 30, 0) };
            var durations = new int[] { 60, 30 };
            var beginWorkingTime = new TimeSpan(8, 0, 0);
            var endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            var calculations = new Calculations();

            // Act
            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // Assert
            CollectionAssert.DoesNotContain(result, "10:30-11:00");
        }
        /// <summary>
        /// Рабочий день заканчивается раньше
        /// </summary>
        [TestMethod]
        public void AvailablePeriods_EndWorkingTimeEarlier_ReturnsValidIntervals()
        {
            // Arrange
            var startTimes = new TimeSpan[] { new TimeSpan(16, 0, 0) };
            var durations = new int[] { 60 };
            var beginWorkingTime = new TimeSpan(8, 0, 0);
            var endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;

            var calculations = new Calculations();

            // Act
            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // Assert
            CollectionAssert.Contains(result, "15:00-15:30");
            CollectionAssert.DoesNotContain(result, "16:30-17:00");
        }

        /// <summary>
        /// Рабочий день начинается позже
        /// </summary>
        [TestMethod]
        public void AvailablePeriods_BeginWorkingTimeLater_ReturnsValidIntervals()
        {
            // Arrange
            var startTimes = new TimeSpan[] { new TimeSpan(12, 0, 0) };
            var durations = new int[] { 60 };
            var beginWorkingTime = new TimeSpan(10, 0, 0);
            var endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            var calculations = new Calculations();

            // Act
            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // Assert
            CollectionAssert.Contains(result, "10:00-10:30");
            CollectionAssert.Contains(result, "11:30-12:00");
        }

        /// <summary>
        /// Консультация длительностью 15 минут
        /// </summary>
        [TestMethod]
        public void AvailablePeriods_ConsultationTime15Minutes_ReturnsValidIntervals()
        {
            // Arrange
            var startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0) };
            var durations = new int[] { 60 };
            var beginWorkingTime = new TimeSpan(8, 0, 0);
            var endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 15;

            var calculations = new Calculations();

            // Act
            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // Assert
            CollectionAssert.Contains(result, "08:00-08:15");
            CollectionAssert.Contains(result, "11:00-11:15");
        }

        /// <summary>
        /// Все промежутки заняты
        /// </summary>
        [TestMethod]
        public void AvailablePeriods_AllIntervalsBusy_ReturnsEmptyList()
        {
            // Arrange
            var startTimes = new TimeSpan[] { new TimeSpan(8, 0, 0) };
            var durations = new int[] { 600 };
            var beginWorkingTime = new TimeSpan(8, 0, 0);
            var endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            var calculations = new Calculations();

            // Act
            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // Assert
            Assert.AreEqual(0, result.Length);
        }
    }
}
