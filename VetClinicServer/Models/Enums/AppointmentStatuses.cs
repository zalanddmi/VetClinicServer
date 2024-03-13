namespace VetClinicServer.Models.Enums
{
    /// <summary>
    /// Статус приема
    /// </summary>
    public enum AppointmentStatuses
    {
        /// <summary>
        /// Не определено
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Черновик
        /// </summary>
        Draft = 1,
        /// <summary>
        /// Ожидание
        /// </summary>
        Waiting = 2,
        /// <summary>
        /// Завершен
        /// </summary>
        Completed = 3,
        /// <summary>
        /// Отменен
        /// </summary>
        Canceled = 4,
        /// <summary>
        /// Удален, в корзине
        /// </summary>
        Deleted = 5
    }
}
