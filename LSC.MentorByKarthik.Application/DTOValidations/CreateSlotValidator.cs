using FluentValidation;
using LSC.MentorByKarthik.Application.DTOs;

namespace LSC.MentorByKarthik.Application.DTOValidations
{
    public class CreateSlotValidator : AbstractValidator<CreateMentoringSlotDto>
    {
        public CreateSlotValidator()
        {
            RuleFor(x => x.SlotDateTime)
                .NotEmpty().WithMessage("Slot date and time is required.")
                .Must(BeInFuture).WithMessage("Slot must be scheduled for a future date and time.");

            RuleFor(x => x.SlotDurationMinutes)
                .GreaterThan(0).WithMessage("Slot duration must be greater than zero.")
                .LessThanOrEqualTo(120).WithMessage("Slot duration cannot exceed 120 minutes.");

            RuleFor(x => x.SlotType)
                .NotEmpty().WithMessage("Slot type is required.")
                .MaximumLength(50).WithMessage("Slot type must not exceed 50 characters.")
                .Must(BeAValidType).WithMessage("Slot type must be one of: VideoCall, WhatsAppCall, GroupCall.");
        }

        private bool BeInFuture(DateTime dateTime)
        {
            return dateTime > DateTime.UtcNow;
        }

        private bool BeAValidType(string slotType)
        {
            var validTypes = new[] { "Google-Meeet", "WhatsApp-Call", "One-to-One-Google-Meet" };
            return validTypes.Contains(slotType);
        }
    }


}
