using AutoMapper;
using BillBudget.Accounts.Logic.ViewModels;
using BillBudget.Application.WebApi.Areas.Accounts.Models;
using BillBudget.Application.WebApi.Dtos;
using BillBudget.Core.Domain.Entities;
using BillBudget.Core.Domain.Entities.Accounts;
using BillBudget.Core.Logic.ViewModels;
using BillBudget.Voucher.Data.Vouchers;
using Microsoft.AspNetCore.Builder;
using EmployeeViewModel = BillBudget.Core.Logic.ViewModels.EmployeeViewModel;

namespace BillBudget.Application.WebApi.Extentions
{
    public static class AutoMapperExtentions
    {
        public static void ConfigureMapper(this IApplicationBuilder app)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<CostCenter, CostCenterCreatingDto>().ReverseMap();
                m.CreateMap<CostCenter, CostCenterUpdatingDto>().ReverseMap();
                m.CreateMap<CostCenter, CostCenterViewModel>().ReverseMap();

                m.CreateMap<Job, JobCreatingDto>().ReverseMap();
                m.CreateMap<Job, JobViewModel>().ReverseMap();
                m.CreateMap<Job, JobUpdatingDto>().ReverseMap();

                m.CreateMap<BudgetMain, BudgetMainViewModel>().ReverseMap();
                m.CreateMap<BudgetMain, BudgetMainUpdatingDto>().ReverseMap();
                m.CreateMap<BudgetMain, BudgetMainCreatingDto>().ReverseMap();
                m.CreateMap<BudgetSub, BudgetSubViewModel>().ReverseMap();
                m.CreateMap<BudgetSub, BudgetSubCreatingDto>().ReverseMap();
                m.CreateMap<BudgetSub, BudgetSubUpdatingDto>().ReverseMap();

                m.CreateMap<AdvanceMain, AdvanceMainCreatingDto>().ReverseMap();
                m.CreateMap<AdvanceMain, AdvanceMainUpdatingDto>().ReverseMap();
                m.CreateMap<AdvanceMain, AdvanceMainGistViewModel>().ReverseMap();
                m.CreateMap<AdvanceMain, AdvanceMainViewModel>().ReverseMap();
                m.CreateMap<AdvanceSub, AdvanceSubCreatingDto>().ReverseMap();
                m.CreateMap<AdvanceSub, AdvanceSubViewModel>().ReverseMap();

                m.CreateMap<WorkOrderMain, WorkOrderMainCreatingDto>().ReverseMap();
                m.CreateMap<WorkOrderMain, WorkOrderMainUpdatingDto>().ReverseMap();
                m.CreateMap<WorkOrderMain, WorkOrderMainViewModel>().ReverseMap();
                m.CreateMap<WorkOrderSub, WorkOrderSubCreatingDto>().ReverseMap();
                m.CreateMap<WorkOrderSub, WorkOrderSubViewModel>().ReverseMap();

                m.CreateMap<Approval, ApprovalCreatingDto>().ReverseMap();
                m.CreateMap<Approval, ApprovalProcessViewModel>().ReverseMap();
                m.CreateMap<Approval, ApprovalProcessViewModel>()
                        .ForMember(opt => opt.JobName, des => des.MapFrom(d => d.Job.JobName))
                        .ForMember(opt => opt.TypeName, des => des.MapFrom(d => d.BudgetPerformType.TypeName))
                        .ForMember(opt => opt.Purpose, des => des.MapFrom(d => d.ApprovalPurpose.Purpose))
                       .ReverseMap();

                m.CreateMap<Approval, ApprovalViewModel>()
                    .ForMember(opt => opt.JobName, des => des.MapFrom(d => d.Job.JobName))
                    .ForMember(opt => opt.ApprovalPurpose, des => des.MapFrom(d => d.ApprovalPurpose.Purpose))
                    .ForMember(opt => opt.BudgetPerformType, des => des.MapFrom(d => d.BudgetPerformType.TypeName))
                    .ForMember(opt => opt.CostCenterName, des => des.MapFrom(d => d.Job.CostCenter.CostCenterName))
                    .ReverseMap();

                m.CreateMap<Accounts_VoucherMaster, VoucherMasterCreatingDto>().ReverseMap();
                m.CreateMap<Accounts_VoucherMaster, VoucherWithFinancialTransactionDto>().ReverseMap();
                m.CreateMap<Accounts_VoucherDetails, VoucherSubCreatingDto>().ReverseMap();
                m.CreateMap<Accounts_VoucherMaster, VoucherMainViewModel>().ReverseMap();
                m.CreateMap<Accounts_VoucherDetails, VoucherSubViewModel>().ReverseMap();

                m.CreateMap<FinancialYears, FinancialYearsViewModel>().ReverseMap();

                m.CreateMap<Projects, ProjectViewModel>().ReverseMap();

                m.CreateMap<MeasurementUnit, MeasurementUnitViewModel>().ReverseMap();

                m.CreateMap<BillMain, BillMainViewModel>().ReverseMap();
                m.CreateMap<BillMain, BillMainUpdatingDto>().ReverseMap();
                m.CreateMap<BillMain, BillMainCreatingDto>().ReverseMap();
                m.CreateMap<BillSub, BillSubCreatingDto>().ReverseMap();
                m.CreateMap<BillSub, BillSubViewModel>().ReverseMap();

                m.CreateMap<FinancialTransactionDetails, VoucherWithFinancialTransactionDto>().ReverseMap();
                m.CreateMap<FinancialTransactionDetails, TransactionDetailsDto>().ReverseMap();
                m.CreateMap<FinancialTransactionDetails, TransactionDetailsViewModel>().ReverseMap();
               // m.CreateMap<Employees, BillBudget.Accounts.Logic.ViewModels.EmployeeViewModel>().ReverseMap();
                m.CreateMap<Employee, EmployeeViewModel>().ReverseMap();

                //m.CreateMap<Employee, Employees>()
                //    .ForMember(opt => opt.EMPLOYEE_ID, des => des.MapFrom(d => d.Id))
                //    .ForMember(opt => opt.EMPLOYEE_NAME, des => des.MapFrom(d => d.Name))
                //    .ForMember(opt => opt.DEPT_NAME, des => des.MapFrom(d => d.CurrentDepartmentName))
                //    .ForMember(opt => opt.REPORTS_TO, des => des.MapFrom(d => d.ReportsTo))
                //    .ForMember(opt => opt.EMAIL, des => des.MapFrom(d => d.Email))
                //    .ForMember(opt => opt.MOBILE, des => des.MapFrom(d => d.Mobile))
                //    .ForMember(opt => opt.ACTIVE, des => des.MapFrom(d => d.Active))
                //    .ForMember(opt => opt.BLOCKED, des => des.MapFrom(d => d.Blocked))
                //    .ForMember(opt => opt.DESIGNATION, des => des.MapFrom(d => d.Designation))
                //    .ForMember(opt => opt.PERSON_ID, des => des.MapFrom(d => d.PersonId))
                //    .ReverseMap();

                m.CreateMap<BudgetPerformType, BudgetPerformTypeViewModel>().ReverseMap();
                //m.CreateMap<CompanyProfile, CompanyProfileCreationDto>()
                //.ForMember(opt => opt.Latitude, des => des.MapFrom(d => d.GoogleMapLocation.Latitude))
                //.ForMember(opt => opt.Longitude, des => des.MapFrom(d => d.GoogleMapLocation.Longitude))
                //.ForMember(opt => opt.Zoom, des => des.MapFrom(d => d.GoogleMapLocation.Zoom))
                //.ReverseMap();

                //m.CreateMap<CompanyProfile, CompanyProfileDisplayDto>()
                //    .ForMember(opt => opt.Latitude, des => des.MapFrom(d => d.GoogleMapLocation.Latitude))
                //    .ForMember(opt => opt.Longitude, des => des.MapFrom(d => d.GoogleMapLocation.Longitude))
                //    .ForMember(opt => opt.Zoom, des => des.MapFrom(d => d.GoogleMapLocation.Zoom));
            });
        }
    }
}
