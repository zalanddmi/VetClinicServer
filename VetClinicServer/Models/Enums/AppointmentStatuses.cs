namespace VetClinicServer.Models.Enums
{
    /// <summary>
    /// Статус приема
    /// </summary>
    public enum AppointmentStatuses
    {
        /// <summary>
        /// Ожидание
        /// </summary>
        Waiting = 0,
        /// <summary>
        /// Завершен
        /// </summary>
        Completed = 1,
        /// <summary>
        /// Отменен
        /// </summary>
        Cancelled = 2
    }
}
