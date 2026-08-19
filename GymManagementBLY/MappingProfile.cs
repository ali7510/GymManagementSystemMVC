using AutoMapper;
using GymManagementBL.ViewModel.HealthRecordViewModels;
using GymManagementBL.ViewModel.MemberPlanViewModels;
using GymManagementBL.ViewModel.MemberSessionViewModel;
using GymManagementBL.ViewModel.MemberViewModel;
using GymManagementBL.ViewModel.PlanViewModels;
using GymManagementBL.ViewModel.SessionViewModels;
using GymManagementBL.ViewModel.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Enum;
using GymManagmentBLL.ViewModels.SessionViewModel;
using System.Numerics;


namespace GymManagementBL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            SessionMappping();
            MemberMapping();
            TrainerMapping();
            PlanMapping();
            MemberPlanMapping();
            BookingMapping();
        }

        private void BookingMapping()
        {
            CreateMap<Session, GetAllMemberSessionsViewModel>()
                .ForMember(dest => dest.Date, option => option.MapFrom(src => DateOnly.FromDateTime(src.StartDate)))
                .ForMember(dest=>dest.SessionId, option => option.MapFrom(src => src.Id))
                .ForMember(dest=>dest.TrainerId, option => option.MapFrom(src => src.Trainer_Id))
                .ForMember(dest => dest.Capacity, option => option.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.Duration, option => option.MapFrom(src => src.EndDate - src.StartDate))
                .ForMember(dest => dest.Time, option => option.MapFrom(src => src.StartDate.TimeOfDay))
                .ForMember(dest => dest.TrainerName, option => option.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.Status, option => option.MapFrom(src => (src.StartDate > DateTime.Now ? "Upcoming" : "Ongoing")))
                .ForMember(dest => dest.TrainerSpeciality, option => option.MapFrom(src => Enum.GetName(typeof(Speciality), src.Trainer.Speciality)));

            CreateMap<CreateMemberSessionViewModel, Booking>().ReverseMap();

            CreateMap<Booking, GetAllMembersInBooking>()
                .ForMember(dest => dest.MemberId, option => option.MapFrom(src => src.MemberId))
                .ForMember(dest => dest.SessionId, option => option.MapFrom(src => src.SessionId))
                .ForMember(dest => dest.MemberName, option => option.MapFrom(src => src.Member == null ? null : src.Member.Name))
                .ForMember(dest=>dest.isAttended, option => option.MapFrom(src=>src.isAttended));

            //CreateMap<Booking, ViewMembersViewModel>()
            //    .ForMember
        }


        private void TrainerMapping()
        {
            CreateMap<Trainer, TrainerViewModel>()
            .ForMember(dest => dest.Speciality, opt => opt.MapFrom(src => Enum.GetName(typeof(Speciality), src.Speciality)))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNo}, {src.Address.Street}, {src.Address.City}"));

            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNo));

            CreateMap<TrainerToUpdateViewModel, Trainer>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    if (dest.Address == null) dest.Address = new Address(); // Prevent NullReference
                    dest.Address.BuildingNo = src.BuildingNumber;
                    dest.Address.City = src.City;
                    dest.Address.Street = src.Street;
                    dest.Updated_At = DateTime.Now;
                });

            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest=>dest.Name, opt=>opt.MapFrom(src=>src.Name))
                .ForMember(dest=>dest.Phone, opt=>opt.MapFrom(src=>src.Phone))
                .ForMember(dest=>dest.Email, opt=>opt.MapFrom(src=>src.Email))
                .ForMember(dest=>dest.DateOfBirth, opt=>opt.MapFrom(src=>src.DateOfBirth))
                .ForMember(dest=>dest.Speciality, opt=>opt.MapFrom(src=>src.Speciality))
                .ForMember(dest=>dest.Gender, opt=>opt.MapFrom(src=>src.Gender))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                 {
                     BuildingNo = src.BuildingNumber,
                     Street = src.Street,
                     City = src.City
                 }));
        }

        private void SessionMappping()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.StartDate, option => option.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, option => option.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Description, option => option.MapFrom(src => src.Description))
                .ForMember(dest => dest.Capacity, option => option.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.Id, option => option.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoryName, option => option.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TrainerName, option => option.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.AvailableSlots, option => option.Ignore());

            CreateMap<CreateSessionViewModel, Session>().ForMember(dest => dest.EndDate, option => option.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Description, option => option.MapFrom(src => src.Description))
                .ForMember(dest => dest.Capacity, option => option.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.Trainer_Id, option => option.MapFrom(src => src.TrainerId))
                .ForMember(dest => dest.Category_Id, option => option.MapFrom(src => src.CategoryId));

            CreateMap<Session, UpdateSessionViewModel>()
                .ForMember(dest => dest.TrainerId, option => option.MapFrom(src => src.Trainer_Id))
                .ForMember(dest => dest.StartDate, option => option.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, option => option.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Description, option => option.MapFrom(src => src.Description));

            CreateMap<UpdateSessionViewModel, Session>()
                .ForMember(dest => dest.Trainer_Id, option => option.MapFrom(src => src.TrainerId))
                .ForMember(dest => dest.StartDate, option => option.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, option => option.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Description, option => option.MapFrom(src => src.Description));


            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));
        }

        private void MemberPlanMapping()
        {
            CreateMap<MemberPlan, GetAllMembershipsViewModel>()
                .ForMember(dest => dest.MemberId, option => option.MapFrom(src => src.MemberId))
                .ForMember(dest => dest.PlanId, option => option.MapFrom(src => src.PlanId))
                .ForMember(dest => dest.MemberName, option => option.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.PlanName, option => option.MapFrom(src => src.Plan.Name))
                .ForMember(dest => dest.StartDate, option => option.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.EndDate, option => option.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Status, option => option.MapFrom(src => ((MembershipStatus)src.Status).ToString()));

            CreateMap<CreateMembershipViewModel, MemberPlan>()
                .ForMember(dest => dest.MemberId, option => option.MapFrom(src => src.MemberId))
                .ForMember(dest => dest.PlanId, option => option.MapFrom(src => src.PlanId))
                .ForMember(dest => dest.CreatedAt, option => option.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, option => option.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Updated_At, option => option.MapFrom(src => src.StartDate));

        }

        private void MemberMapping()
        {
            CreateMap<Member, HealthRecordViewModel>()
                .ForMember(dest => dest.Weight, option => option.MapFrom(src => src.HealthRecord.Weight))
                .ForMember(dest => dest.Height, option => option.MapFrom(src => src.HealthRecord.Height))
                .ForMember(dest => dest.BloodType, option => option.MapFrom(src => src.HealthRecord.BloodType))
                .ForMember(dest => dest.Note, option => option.MapFrom(src => src.HealthRecord.Note));

            CreateMap<Member, UpdateMemberViewModel>().ForMember(dest => dest.BuildingNumber, option => option.MapFrom(src => src.Address.BuildingNo))
                .ForMember(dest => dest.Street, option => option.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, option => option.MapFrom(src => src.Address.City));

            CreateMap<Member, GetMemberDetailsViewModel>().ForMember(dest => dest.Gender, option => option.MapFrom(src => src.Gender.ToString()))
            .ForMember(dest => dest.BuildinhNo, option => option.MapFrom(src => src.Address.BuildingNo))
            .ForMember(dest => dest.Street, option => option.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.City, option => option.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Photo, option => option.MapFrom(src => src.Photo))
            .ForMember(dest => dest.Phone, option => option.MapFrom(src => src.Phone))
            .ForMember(dest => dest.Email, option => option.MapFrom(src => src.Email))
            .ForMember(dest => dest.Name, option => option.MapFrom(src => src.Name))
            .ForMember(dest => dest.DateOfBirth, option => option.MapFrom(src => src.DateOfBirth));

            CreateMap<UpdateMemberViewModel, Member>()
                .ForMember(dest => dest.Email, option => option.MapFrom(src => src.Email))
                .ForMember(dest => dest.Phone, option => option.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Address, option => option.MapFrom(src => new Address
                {
                    City = src.City,
                    Street = src.Street,
                    BuildingNo = src.BuildingNumber
                }));

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, option => option.MapFrom(src => new Address
                {
                    City = src.City,
                    Street = src.Street,
                    BuildingNo = src.BuildingNumber
                }))
                .ForMember(dest => dest.HealthRecord, option => option.MapFrom(src => new HealthRecord
                {
                    Weight = src.HealthRecord.Weight,
                    Height = src.HealthRecord.Weight,
                    BloodType = src.HealthRecord.BloodType,
                    Note = src.HealthRecord.Note,

                }));
        }

        private void PlanMapping()
        {
            CreateMap<Plan, GetAllPlansViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>().ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name));
            CreateMap<UpdatePlanViewModel, Plan>()
           .ForMember(dest => dest.Name, opt => opt.Ignore())
           .ForMember(dest => dest.Updated_At, opt => opt.MapFrom(src => DateTime.Now));
        }


    }
}
