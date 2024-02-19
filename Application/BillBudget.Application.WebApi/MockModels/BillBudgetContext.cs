using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class BillBudgetContext : DbContext
    {
        public BillBudgetContext()
        {
        }

        public BillBudgetContext(DbContextOptions<BillBudgetContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdvanceMain> AdvanceMain { get; set; }
        public virtual DbSet<AdvanceSub> AdvanceSub { get; set; }
        public virtual DbSet<Approval> Approval { get; set; }
        public virtual DbSet<ApprovalCommittee> ApprovalCommittee { get; set; }
        public virtual DbSet<ApprovalPurpose> ApprovalPurpose { get; set; }
        public virtual DbSet<BillMain> BillMain { get; set; }
        public virtual DbSet<BillSub> BillSub { get; set; }
        public virtual DbSet<BudgetMain> BudgetMain { get; set; }
        public virtual DbSet<BudgetPerformType> BudgetPerformType { get; set; }
        public virtual DbSet<BudgetSub> BudgetSub { get; set; }
        public virtual DbSet<CostCenter> CostCenter { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<FinancialTransactionDetails> FinancialTransactionDetails { get; set; }
        public virtual DbSet<FinancialTransactionType> FinancialTransactionType { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<MeasurementUnit> MeasurementUnit { get; set; }
        public virtual DbSet<WorkforProxy> WorkforProxy { get; set; }
        public virtual DbSet<WorkOrderMain> WorkOrderMain { get; set; }
        public virtual DbSet<WorkOrderSub> WorkOrderSub { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("User Id=sa; password=Start777; initial catalog=BillBudget; data source=203.190.8.5\\commvault");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AdvanceMain>(entity =>
            {
                entity.ToTable("AdvanceMain", "BillBudget");

                entity.HasIndex(e => e.JobId);

                entity.HasIndex(e => e.WorkOrderMainId);

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .ValueGeneratedNever();

                entity.Property(e => e.AdvanceName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.ApplicationStatus).HasMaxLength(100);

                entity.Property(e => e.AssignTo).HasMaxLength(10);

                entity.Property(e => e.CreatedBy).HasMaxLength(10);

                entity.Property(e => e.ForwardTo).HasMaxLength(10);

                entity.Property(e => e.JobId).HasMaxLength(36);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.QuotationNo).HasMaxLength(100);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.Property(e => e.VendorDetail).HasMaxLength(1000);

                entity.Property(e => e.VendorName).HasMaxLength(1000);

                entity.Property(e => e.WorkOrderMainId).HasMaxLength(36);

                entity.HasOne(d => d.AssignToNavigation)
                    .WithMany(p => p.AdvanceMainAssignToNavigation)
                    .HasForeignKey(d => d.AssignTo)
                    .HasConstraintName("FK_AdvanceMain_Employee1");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AdvanceMainCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_AdvanceMain_Employee");

                entity.HasOne(d => d.ForwardToNavigation)
                    .WithMany(p => p.AdvanceMainForwardToNavigation)
                    .HasForeignKey(d => d.ForwardTo)
                    .HasConstraintName("FK_AdvanceMain_Employee2");
                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.AdvanceMainUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_AdvanceMain_Employee3");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.AdvanceMain)
                    .HasForeignKey(d => d.JobId);

              
                entity.HasOne(d => d.WorkOrderMain)
                    .WithMany(p => p.AdvanceMain)
                    .HasForeignKey(d => d.WorkOrderMainId);
            });

            builder.Entity<AdvanceSub>(entity =>
            {
                entity.ToTable("AdvanceSub", "BillBudget");

                entity.HasIndex(e => e.AdvanceMainId);

                entity.HasIndex(e => e.MeasurementUnitId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AdvanceMainId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.CreatedBy).HasMaxLength(10);

                entity.Property(e => e.ItemName).HasMaxLength(250);

                entity.Property(e => e.MeasurementUnitId).HasMaxLength(36);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.HasOne(d => d.AdvanceMain)
                    .WithMany(p => p.AdvanceSub)
                    .HasForeignKey(d => d.AdvanceMainId);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AdvanceSubCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_AdvanceSub_Employee");

                entity.HasOne(d => d.MeasurementUnit)
                    .WithMany(p => p.AdvanceSub)
                    .HasForeignKey(d => d.MeasurementUnitId);

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.AdvanceSubUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_AdvanceSub_Employee1");
            });

            builder.Entity<Approval>(entity =>
            {
                entity.ToTable("Approval", "BillBudget");

                entity.HasIndex(e => e.ApprovalPurposeId);

                entity.HasIndex(e => e.BudgetPerformTypeId);

                entity.HasIndex(e => e.JobId);

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .ValueGeneratedNever();

                entity.Property(e => e.ApprovalPurposeId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.AssignTo)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.BudgetPerformTypeId).HasMaxLength(36);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ForwardTo).HasMaxLength(10);

                entity.Property(e => e.JobId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.RefId)
                    .IsRequired()
                    .HasColumnName("Ref_Id")
                    .HasMaxLength(36);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.Property(e => e.VarificationKey).HasMaxLength(200);

                entity.HasOne(d => d.ApprovalPurpose)
                    .WithMany(p => p.Approval)
                    .HasForeignKey(d => d.ApprovalPurposeId);

                entity.HasOne(d => d.AssignToNavigation)
                    .WithMany(p => p.ApprovalAssignToNavigation)
                    .HasForeignKey(d => d.AssignTo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Approval_Employee1");

                entity.HasOne(d => d.BudgetPerformType)
                    .WithMany(p => p.Approval)
                    .HasForeignKey(d => d.BudgetPerformTypeId);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ApprovalCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Approval_Employee");

                entity.HasOne(d => d.ForwardToNavigation)
                    .WithMany(p => p.ApprovalForwardToNavigation)
                    .HasForeignKey(d => d.ForwardTo)
                    .HasConstraintName("FK_Approval_Employee2");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.Approval)
                    .HasForeignKey(d => d.JobId);

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.ApprovalUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Approval_Employee3");
            });

            builder.Entity<ApprovalCommittee>(entity =>
            {
                entity.ToTable("ApprovalCommittee", "BillBudget");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(10);

                entity.Property(e => e.EmployeeId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ApprovalCommitteeCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_ApprovalCommittee_Employee");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.ApprovalCommitteeUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_ApprovalCommittee_Employee1");
            });

            builder.Entity<ApprovalPurpose>(entity =>
            {
                entity.ToTable("ApprovalPurpose", "BillBudget");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(10);

                entity.Property(e => e.Purpose)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ApprovalPurposeCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_ApprovalPurpose_Employee");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.ApprovalPurposeUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_ApprovalPurpose_Employee1");
            });

            builder.Entity<BillMain>(entity =>
            {
                entity.ToTable("BillMain", "BillBudget");

                entity.HasIndex(e => e.JobId);

                entity.HasIndex(e => e.WorkOrderMainId);

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .ValueGeneratedNever();

                entity.Property(e => e.ApplicationStatus).HasMaxLength(100);

                entity.Property(e => e.AssignTo).HasMaxLength(10);

                entity.Property(e => e.BillName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreatedBy).HasMaxLength(10);

                entity.Property(e => e.ForwardTo).HasMaxLength(10);

                entity.Property(e => e.JobId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.QoutationNo).HasMaxLength(100);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.Property(e => e.VendorName).HasMaxLength(250);

                entity.Property(e => e.VoucherNo).HasMaxLength(50);

                entity.Property(e => e.WorkOrderMainId).HasMaxLength(36);

                entity.HasOne(d => d.AssignToNavigation)
                    .WithMany(p => p.BillMainAssignToNavigation)
                    .HasForeignKey(d => d.AssignTo)
                    .HasConstraintName("FK_BillMain_Employee1");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.BillMainCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_BillMain_Employee");

                entity.HasOne(d => d.ForwardToNavigation)
                    .WithMany(p => p.BillMainForwardToNavigation)
                    .HasForeignKey(d => d.ForwardTo)
                    .HasConstraintName("FK_BillMain_Employee2");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.BillMain)
                    .HasForeignKey(d => d.JobId);

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.BillMainUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_BillMain_Employee3");

                entity.HasOne(d => d.WorkOrderMain)
                    .WithMany(p => p.BillMain)
                    .HasForeignKey(d => d.WorkOrderMainId);
            });

            builder.Entity<BillSub>(entity =>
            {
                entity.ToTable("BillSub", "BillBudget");

                entity.HasIndex(e => e.BillMainId);

                entity.HasIndex(e => e.MeasurementUnitId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BillMainId).HasMaxLength(36);

                entity.Property(e => e.CreatedBy).HasMaxLength(10);

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.MeasurementUnitId).HasMaxLength(36);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.HasOne(d => d.BillMain)
                    .WithMany(p => p.BillSub)
                    .HasForeignKey(d => d.BillMainId);

                entity.HasOne(d => d.MeasurementUnit)
                    .WithMany(p => p.BillSub)
                    .HasForeignKey(d => d.MeasurementUnitId);

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.BillSub)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_BillSub_Employee1");
            });

            builder.Entity<BudgetMain>(entity =>
            {
                entity.ToTable("BudgetMain", "BillBudget");

                entity.HasIndex(e => e.JobId);

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .ValueGeneratedNever();

                entity.Property(e => e.ApplicationStatus).HasMaxLength(100);

                entity.Property(e => e.AssignTo).HasMaxLength(10);

                entity.Property(e => e.BudgetName).HasMaxLength(250);

                entity.Property(e => e.CreatedBy).HasMaxLength(10);

                entity.Property(e => e.ForwardTo).HasMaxLength(10);

                entity.Property(e => e.JobId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.HasOne(d => d.AssignToNavigation)
                    .WithMany(p => p.BudgetMainAssignToNavigation)
                    .HasForeignKey(d => d.AssignTo)
                    .HasConstraintName("FK_BudgetMain_Employee1");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.BudgetMainCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_BudgetMain_Employee");

                entity.HasOne(d => d.ForwardToNavigation)
                    .WithMany(p => p.BudgetMainForwardToNavigation)
                    .HasForeignKey(d => d.ForwardTo)
                    .HasConstraintName("FK_BudgetMain_Employee2");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.BudgetMain)
                    .HasForeignKey(d => d.JobId);

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.BudgetMainUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_BudgetMain_Employee3");
            });

            builder.Entity<BudgetPerformType>(entity =>
            {
                entity.ToTable("BudgetPerformType", "BillBudget");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(10);

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.BudgetPerformTypeCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_BudgetPerformType_Employee");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.BudgetPerformTypeUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_BudgetPerformType_Employee1");
            });

            builder.Entity<BudgetSub>(entity =>
            {
                entity.ToTable("BudgetSub", "BillBudget");

                entity.HasIndex(e => e.BudgetMainId);

                entity.HasIndex(e => e.MeasurementUnitId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BudgetMainId).HasMaxLength(36);

                entity.Property(e => e.CreatedBy).HasMaxLength(10);

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.MeasurementUnitId).HasMaxLength(36);

                entity.Property(e => e.QoutationNo).HasMaxLength(100);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.Property(e => e.VendorName).HasMaxLength(250);

                entity.Property(e => e.WorkOrderNo).HasMaxLength(100);

                entity.HasOne(d => d.BudgetMain)
                    .WithMany(p => p.BudgetSub)
                    .HasForeignKey(d => d.BudgetMainId);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.BudgetSubCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_BudgetSub_Employee");

                entity.HasOne(d => d.MeasurementUnit)
                    .WithMany(p => p.BudgetSub)
                    .HasForeignKey(d => d.MeasurementUnitId);

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.BudgetSubUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_BudgetSub_Employee1");
            });

            builder.Entity<CostCenter>(entity =>
            {
                entity.ToTable("CostCenter", "BillBudget");

                entity.HasIndex(e => e.CostCenterName)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .ValueGeneratedNever();

                entity.Property(e => e.ApprovalAuthority).HasMaxLength(36);

                entity.Property(e => e.CostCenterName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreatedBy).HasMaxLength(10);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CostCenterCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CostCenter_Employee");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CostCenterUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_CostCenter_Employee1");
            });

            builder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.Active).HasMaxLength(1);

                entity.Property(e => e.Blocked).HasMaxLength(1);

                entity.Property(e => e.CurrentDepartmentName).HasMaxLength(50);

                entity.Property(e => e.Designation).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ReportsTo).HasMaxLength(10);

                entity.HasOne(d => d.ReportsToNavigation)
                    .WithMany(p => p.InverseReportsToNavigation)
                    .HasForeignKey(d => d.ReportsTo)
                    .HasConstraintName("FK_BillBudget.Employee_BillBudget.Employee");
            });

            builder.Entity<FinancialTransactionDetails>(entity =>
            {
                entity.ToTable("FinancialTransactionDetails", "BillBudget");

                entity.HasIndex(e => e.FinancialTransactionTypeId);

                entity.HasIndex(e => e.JobId);

                entity.HasIndex(e => e.WorkOrderMainId);

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountHead).HasMaxLength(100);

                entity.Property(e => e.BillType).HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasMaxLength(10);

                entity.Property(e => e.FinancialTransactionTypeId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.JobId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.ReferenceId).HasMaxLength(36);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.Property(e => e.VoucherNo).HasMaxLength(50);

                entity.Property(e => e.WorkOrderMainId).HasMaxLength(36);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.FinancialTransactionDetailsCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_FinancialTransactionDetails_Employee");

                entity.HasOne(d => d.FinancialTransactionType)
                    .WithMany(p => p.FinancialTransactionDetails)
                    .HasForeignKey(d => d.FinancialTransactionTypeId);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.FinancialTransactionDetails)
                    .HasForeignKey(d => d.JobId);

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.FinancialTransactionDetailsUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_FinancialTransactionDetails_Employee1");

                entity.HasOne(d => d.WorkOrderMain)
                    .WithMany(p => p.FinancialTransactionDetails)
                    .HasForeignKey(d => d.WorkOrderMainId);
            });

            builder.Entity<FinancialTransactionType>(entity =>
            {
                entity.ToTable("FinancialTransactionType", "BillBudget");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.FinancialTransactionTypeCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_FinancialTransactionType_Employee");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.FinancialTransactionTypeUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_FinancialTransactionType_Employee1");
            });

            builder.Entity<Job>(entity =>
            {
                entity.ToTable("Job", "BillBudget");

                entity.HasIndex(e => e.CostCenterId);

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .ValueGeneratedNever();

                entity.Property(e => e.AssignTo)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.CostCenterId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.CreatedBy).HasMaxLength(10);

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.JobSupervisorId).HasMaxLength(10);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.ReferenceNo).HasMaxLength(100);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.HasOne(d => d.AssignToNavigation)
                    .WithMany(p => p.JobAssignToNavigation)
                    .HasForeignKey(d => d.AssignTo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Job_Employee1");

                entity.HasOne(d => d.CostCenter)
                    .WithMany(p => p.Job)
                    .HasForeignKey(d => d.CostCenterId);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.JobCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Job_Employee");

                entity.HasOne(d => d.JobSupervisor)
                    .WithMany(p => p.JobJobSupervisor)
                    .HasForeignKey(d => d.JobSupervisorId)
                    .HasConstraintName("FK_Job_Employee2");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.JobUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Job_Employee3");
            });

            builder.Entity<MeasurementUnit>(entity =>
            {
                entity.ToTable("MeasurementUnit", "BillBudget");

                entity.HasIndex(e => e.Unit)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(10);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.MeasurementUnitCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_MeasurementUnit_Employee");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.MeasurementUnitUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_MeasurementUnit_Employee1");
            });

            builder.Entity<WorkforProxy>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.WorkforId });

                entity.Property(e => e.EmployeeId).HasMaxLength(10);

                entity.Property(e => e.WorkforId).HasMaxLength(10);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.WorkforProxyEmployee)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BillBudget.WorkforProxy_BillBudget.Employee");

                entity.HasOne(d => d.Workfor)
                    .WithMany(p => p.WorkforProxyWorkfor)
                    .HasForeignKey(d => d.WorkforId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BillBudget.WorkforProxy_BillBudget.Employee1");
            });

            builder.Entity<WorkOrderMain>(entity =>
            {
                entity.ToTable("WorkOrderMain", "BillBudget");

                entity.HasIndex(e => e.JobId);

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .ValueGeneratedNever();

                entity.Property(e => e.ContactPerson).HasMaxLength(250);

                entity.Property(e => e.CreatedBy).HasMaxLength(10);

                entity.Property(e => e.DelivaryPlace).HasMaxLength(2000);

                entity.Property(e => e.JobId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.TermsCondition).HasMaxLength(2000);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.Property(e => e.VendorDetail).HasMaxLength(250);

                entity.Property(e => e.VendorName).HasMaxLength(250);

                entity.Property(e => e.WorkOrderName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.WorkOrderNumber).HasMaxLength(150);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.WorkOrderMainCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_WorkOrderMain_Employee");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.WorkOrderMain)
                    .HasForeignKey(d => d.JobId);

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.WorkOrderMainUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_WorkOrderMain_Employee1");
            });

            builder.Entity<WorkOrderSub>(entity =>
            {
                entity.ToTable("WorkOrderSub", "BillBudget");

                entity.HasIndex(e => e.MeasurementUnitId);

                entity.HasIndex(e => e.WorkOrderMainId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(10);

                entity.Property(e => e.MeasurementUnitId).HasMaxLength(36);

                entity.Property(e => e.UpdatedBy).HasMaxLength(10);

                entity.Property(e => e.WorkOrderMainId).HasMaxLength(36);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.WorkOrderSubCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_WorkOrderSub_Employee");

                entity.HasOne(d => d.MeasurementUnit)
                    .WithMany(p => p.WorkOrderSub)
                    .HasForeignKey(d => d.MeasurementUnitId);

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.WorkOrderSubUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_WorkOrderSub_Employee1");

                entity.HasOne(d => d.WorkOrderMain)
                    .WithMany(p => p.WorkOrderSub)
                    .HasForeignKey(d => d.WorkOrderMainId);
            });
        }
    }
}
