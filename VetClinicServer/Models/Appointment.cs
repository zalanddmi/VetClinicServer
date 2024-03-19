﻿using System.ComponentModel.DataAnnotations.Schema;
using VetClinicServer.Models.Enums;

namespace VetClinicServer.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int ProcedureId { get; set; }
        [ForeignKey("ProcedureId")]
        public required Procedure Procedure { get; set; }
        public int PetId { get; set; }
        [ForeignKey("PetId")]
        public required Pet Pet { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public required User User { get; set; }
        public string? Description { get; set; }
        public DateTime DateAppointment { get; set; }
        public AppointmentStatuses Status { get; set; } = AppointmentStatuses.Undefined;
        public DateTime? DateCompleted { get; set; }
        public decimal TotalCost { get; set; } = 0;

        public ICollection<AppointmentDrug> AppointmentDrugs { get; set; }
    }
}