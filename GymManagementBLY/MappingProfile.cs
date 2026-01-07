using AutoMapper;
using GymManagementBL.ViewModel.HealthRecordViewModels;
using GymManagementBL.ViewModel.MemberViewModel;
using GymManagementBL.ViewModel.PlanViewModels;
using GymManagementBL.ViewModel.SessionViewModels;
using GymManagementBL.ViewModel.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Enum;
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
        }

        private void SessionMappping()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, option => option.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TrainerName, option => option.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.AvailableSlots, option => option.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
            CreateMap<Member, GetMemberDetailsViewModel>().ForMember(dest => dest.Gender, option => option.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.BuildinhNo, option => option.MapFrom(src => src.Address.BuildingNo))
                .ForMember(dest => dest.Street, option => option.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, option => option.MapFrom(src => src.Address.City));
        }

        public void MemberMapping()
        {
            CreateMap<Member, HealthRecordViewModel>()
                .ForMember(dest => dest.Weight, option => option.MapFrom(src => src.HealthRecord.Weight))
                .ForMember(dest => dest.Height, option => option.MapFrom(src => src.HealthRecord.Height))
                .ForMember(dest => dest.BloodType, option => option.MapFrom(src => src.HealthRecord.BloodType))
                .ForMember(dest => dest.Note, option => option.MapFrom(src => src.HealthRecord.Note));

            CreateMap<Member, UpdateMemberViewModel>().ForMember(dest => dest.BuildingNumber, option => option.MapFrom(src => src.Address.BuildingNo))
                .ForMember(dest => dest.Street, option => option.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, option => option.MapFrom(src => src.Address.City));

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

        //private void MapPlan()
        //{
        //    CreateMap<Plane, PlanViewModel>();
        //    CreateMap<Plane, UpdatePlanViewModel>().ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name));
        //    CreateMap<UpdatePlanViewModel, Plane>()
        //   .ForMember(dest => dest.Name, opt => opt.Ignore())
        //   .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

        //}


    }
}
