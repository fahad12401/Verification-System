namespace VerificationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressId = c.Long(nullable: false, identity: true),
                        InquiryId = c.Long(nullable: false),
                        VerificationType = c.String(),
                        PersonType = c.String(),
                        Address1 = c.String(nullable: false),
                        Address2 = c.String(),
                        Address3 = c.String(nullable: false),
                        Descriptions = c.String(),
                        City = c.String(),
                        Province = c.String(),
                        RecordedBy = c.String(),
                        RecordAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.Inquiries", t => t.InquiryId)
                .Index(t => t.InquiryId);
            
            CreateTable(
                "dbo.Inquiries",
                c => new
                    {
                        InquiryId = c.Long(nullable: false, identity: true),
                        CustomerCode = c.String(),
                        CompanyId = c.Long(nullable: false),
                        CompanyName = c.String(),
                        BranchId = c.Long(nullable: false),
                        BranchName = c.String(),
                        CustomerBranchId = c.Long(nullable: false),
                        CustomerBranchName = c.String(),
                        ProductId = c.Long(nullable: false),
                        AppNo = c.String(),
                        AppName = c.String(),
                        AppContact = c.String(),
                        AppCNIC = c.String(),
                        Price = c.Double(),
                        Status = c.String(),
                        StatusDate = c.DateTime(nullable: false),
                        StatusDescription = c.String(),
                        RecordAt = c.DateTime(nullable: false),
                        RecordBy = c.String(),
                        UpdateAt = c.DateTime(),
                        UpdateBy = c.String(),
                        FileName = c.String(),
                        BatchNo = c.String(),
                        IsExported = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.InquiryId)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.CustomerBranches", t => t.CustomerBranchId)
                .Index(t => t.CompanyId)
                .Index(t => t.BranchId)
                .Index(t => t.CustomerBranchId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.BankStatementVerifications",
                c => new
                    {
                        BankStatementVerificationId = c.Long(nullable: false, identity: true),
                        InquiryId = c.Long(nullable: false),
                        PersonType = c.String(),
                        PersonName = c.String(),
                        PersonContactNo = c.String(),
                        BankName = c.String(),
                        BankAddress = c.String(),
                        NearestLandMark = c.String(),
                        GeneralComments = c.String(),
                        OutComeVerification = c.String(),
                        GpsLocation = c.String(),
                        GpsURL = c.String(),
                        Status = c.String(),
                        StatusDate = c.DateTime(nullable: false),
                        Price = c.Double(),
                        QCComments = c.String(),
                        VerifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.BankStatementVerificationId)
                .ForeignKey("dbo.Inquiries", t => t.InquiryId)
                .Index(t => t.InquiryId);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        BranchId = c.Long(nullable: false),
                        Name = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        Country = c.String(),
                        City = c.String(),
                        Province = c.String(),
                        Phone1 = c.String(),
                        Phone2 = c.String(),
                        Email = c.String(),
                        WebAddress = c.String(),
                        DisableDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BranchId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 50, unicode: false),
                        LastName = c.String(nullable: false, maxLength: 50, unicode: false),
                        CompanyId = c.Long(),
                        BranchId = c.Long(),
                        CustomerBranchId = c.Long(),
                        Address = c.String(),
                        CustomerCode = c.String(),
                        IsHead = c.Boolean(nullable: false),
                        ParentCode = c.Long(),
                        Code = c.String(nullable: false),
                        DisableDate = c.DateTime(),
                        RecordBy = c.String(),
                        RecordAt = c.DateTime(nullable: false),
                        UpdateBy = c.String(),
                        UpdateAt = c.DateTime(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.CustomerBranches", t => t.CustomerBranchId)
                .Index(t => t.CompanyId)
                .Index(t => t.BranchId)
                .Index(t => t.CustomerBranchId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        Country = c.String(),
                        City = c.String(),
                        Province = c.String(),
                        Phone1 = c.String(),
                        Phone2 = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        WebAddress = c.String(),
                        DisableDate = c.DateTime(),
                        Descriptions = c.String(),
                        RecordBy = c.String(),
                        UpdateBy = c.String(),
                        RecordAt = c.DateTime(nullable: false),
                        UpdateAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CompanyId = c.Long(nullable: false),
                        RecordBy = c.String(),
                        RecordAt = c.DateTime(nullable: false),
                        UpdateBy = c.String(),
                        UpdateAt = c.DateTime(),
                        Sms = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.CustomerBranches",
                c => new
                    {
                        CustomerBranchId = c.Long(nullable: false, identity: true),
                        CompanyId = c.Long(nullable: false),
                        Name = c.String(),
                        Address1 = c.String(),
                        Country = c.String(),
                        City = c.String(),
                        Province = c.String(),
                        Phone1 = c.String(),
                        Phone2 = c.String(),
                        Email = c.String(),
                        DisableDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CustomerBranchId)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.InquiryApplicationUsers",
                c => new
                    {
                        InquiryId = c.Long(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        VerificationType = c.String(nullable: false, maxLength: 128),
                        PersonType = c.String(nullable: false, maxLength: 128),
                        VerificationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.InquiryId, t.UserId, t.VerificationType, t.PersonType, t.VerificationId })
                .ForeignKey("dbo.Inquiries", t => t.InquiryId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.InquiryId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.InquiryImages",
                c => new
                    {
                        InquiryImageId = c.Long(nullable: false, identity: true),
                        InquiryId = c.Long(nullable: false),
                        ImagePath = c.String(),
                        VerificationType = c.String(),
                        PersonType = c.String(),
                        VerificationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.InquiryImageId)
                .ForeignKey("dbo.Inquiries", t => t.InquiryId)
                .Index(t => t.InquiryId);
            
            CreateTable(
                "dbo.ResidenceVerifications",
                c => new
                    {
                        ResidenceVerificationId = c.Long(nullable: false, identity: true),
                        InquiryId = c.Long(nullable: false),
                        PersonType = c.String(),
                        PersonName = c.String(),
                        PersonCNIC = c.String(),
                        PersonContactNo = c.String(),
                        ResidenceAddress = c.String(),
                        NearestLandMark = c.String(),
                        GeneralComments = c.String(),
                        OutComeVerification = c.String(),
                        GpsLocation = c.String(),
                        GpsURL = c.String(),
                        Status = c.String(),
                        StatusDate = c.DateTime(nullable: false),
                        Price = c.Double(),
                        QCComments = c.String(),
                        VerifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.ResidenceVerificationId)
                .ForeignKey("dbo.Inquiries", t => t.InquiryId)
                .Index(t => t.InquiryId);
            
            CreateTable(
                "dbo.NeighbourChecks",
                c => new
                    {
                        NeighbourCheckId = c.Long(nullable: false),
                        NeighborsName = c.String(),
                        NeighborsAddress = c.String(),
                        KnowsApplicant = c.Boolean(nullable: false),
                        KnowsHowLong = c.String(),
                        NeighborComments = c.String(),
                        NeighborsName2 = c.String(),
                        NeighborsAddress2 = c.String(),
                        KnowsApplicant2 = c.Boolean(nullable: false),
                        KnowsHowLong2 = c.String(),
                        NeighborComments2 = c.String(),
                    })
                .PrimaryKey(t => t.NeighbourCheckId)
                .ForeignKey("dbo.ResidenceVerifications", t => t.NeighbourCheckId)
                .Index(t => t.NeighbourCheckId);
            
            CreateTable(
                "dbo.ResidenceDetails",
                c => new
                    {
                        ResidenceDetailId = c.Long(nullable: false),
                        IsApplicantAvailable = c.Boolean(nullable: false),
                        NameOfPersonToMet = c.String(),
                        RelationWithApplicant = c.String(),
                        WasActualAddressSame = c.Boolean(nullable: false),
                        CorrectAddress = c.String(),
                        PhoneNo = c.String(),
                        LivesAtGivenAddress = c.Boolean(nullable: false),
                        PermanentAddress = c.String(),
                        SinceHowLongLiving = c.String(),
                        CNICNo = c.String(),
                        ReferenceRelation = c.String(),
                        SinceHowLongReferenceKnows = c.String(),
                    })
                .PrimaryKey(t => t.ResidenceDetailId)
                .ForeignKey("dbo.ResidenceVerifications", t => t.ResidenceDetailId)
                .Index(t => t.ResidenceDetailId);
            
            CreateTable(
                "dbo.ResidenceProfiles",
                c => new
                    {
                        ResidenceVerificationId = c.Long(nullable: false),
                        TypeOfResidence = c.String(),
                        ApplicantIsA = c.String(),
                        MentionOther = c.String(),
                        MentionRent = c.Double(),
                        SizeApproxArea = c.String(),
                        UtilizationOfResidence = c.String(),
                        RentDeedVerified = c.String(),
                        Neighborhood = c.String(),
                        AreaAccessibility = c.String(),
                        ResidentsBelongsTo = c.String(),
                        RepossessionInTheArea = c.String(),
                    })
                .PrimaryKey(t => t.ResidenceVerificationId)
                .ForeignKey("dbo.ResidenceVerifications", t => t.ResidenceVerificationId)
                .Index(t => t.ResidenceVerificationId);
            
            CreateTable(
                "dbo.SalarySlipVerifications",
                c => new
                    {
                        SalarySlipVerificationId = c.Long(nullable: false, identity: true),
                        InquiryId = c.Long(nullable: false),
                        PersonType = c.String(),
                        PersonName = c.String(),
                        PersonContactNo = c.String(),
                        OfficeName = c.String(),
                        OfficeAddress = c.String(),
                        NearestLandMark = c.String(),
                        PaySlipPath = c.String(),
                        GeneralComments = c.String(),
                        OutComeVerification = c.String(),
                        GpsLocation = c.String(),
                        GpsURL = c.String(),
                        Status = c.String(),
                        StatusDate = c.DateTime(nullable: false),
                        QCComments = c.String(),
                        Price = c.Double(),
                        VerifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.SalarySlipVerificationId)
                .ForeignKey("dbo.Inquiries", t => t.InquiryId)
                .Index(t => t.InquiryId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        StatusId = c.Long(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        InquiryId = c.Long(nullable: false),
                        StatusMessage = c.String(),
                        StatusDate = c.DateTime(nullable: false),
                        VerificationType = c.String(),
                        PersonType = c.String(),
                        Descriptions = c.String(),
                        VerificationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Inquiries", t => t.InquiryId)
                .Index(t => t.UserId)
                .Index(t => t.InquiryId);
            
            CreateTable(
                "dbo.TenantVerifications",
                c => new
                    {
                        TenantVerificationId = c.Long(nullable: false, identity: true),
                        InquiryId = c.Long(nullable: false),
                        PersonType = c.String(),
                        PersonName = c.String(),
                        PersonContactNo = c.String(),
                        TenantAddress = c.String(),
                        NearestLandMark = c.String(),
                        TenantName = c.String(),
                        TenantContactNo = c.String(),
                        TenantCNIC = c.String(),
                        TenancyPeriod = c.String(),
                        TenantRent = c.Double(nullable: false),
                        Status = c.String(),
                        StatusDate = c.DateTime(nullable: false),
                        GeneralComments = c.String(),
                        OutComeVerification = c.String(),
                        GpsLocation = c.String(),
                        GpsURL = c.String(),
                        Price = c.Double(),
                        QCComments = c.String(),
                        VerifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.TenantVerificationId)
                .ForeignKey("dbo.Inquiries", t => t.InquiryId)
                .Index(t => t.InquiryId);
            
            CreateTable(
                "dbo.WorkOfficeVerifications",
                c => new
                    {
                        WorkOfficeVerificationId = c.Long(nullable: false, identity: true),
                        InquiryId = c.Long(nullable: false),
                        PersonType = c.String(),
                        PersonName = c.String(),
                        PersonContactNo = c.String(),
                        PersonDesignation = c.String(),
                        OfficeName = c.String(),
                        OfficeAddress = c.String(),
                        NearestLandMark = c.String(),
                        GeneralComments = c.String(),
                        OutComeVerification = c.String(),
                        GpsLocation = c.String(),
                        GpsURL = c.String(),
                        Status = c.String(),
                        StatusDate = c.DateTime(nullable: false),
                        QCComments = c.String(),
                        Price = c.Double(),
                        VerifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.WorkOfficeVerificationId)
                .ForeignKey("dbo.Inquiries", t => t.InquiryId)
                .Index(t => t.InquiryId);
            
            CreateTable(
                "dbo.BusinessWorkOfficeDetails",
                c => new
                    {
                        BusinessWorkOfficeDetailId = c.Long(nullable: false),
                        TypeOfBusiness = c.String(),
                        OtherTypeOfBusiness = c.String(),
                        ApplicantIsA = c.String(),
                        MentionOther = c.String(),
                        MentionRent = c.String(),
                        NatureOfBusiness = c.String(),
                        OtherNatureOfBusiness = c.String(),
                        BusinessLegalEntity = c.String(),
                        GovtEmpBusinessLegalEntity = c.String(),
                        NamePlateAffixed = c.Boolean(nullable: false),
                        SizeApproxArea = c.String(),
                        BusinessActivity = c.String(),
                        NoOfEmployees = c.Int(nullable: false),
                        BusinessEstablesSince = c.String(),
                        LineOfBusiness = c.String(),
                    })
                .PrimaryKey(t => t.BusinessWorkOfficeDetailId)
                .ForeignKey("dbo.WorkOfficeVerifications", t => t.BusinessWorkOfficeDetailId)
                .Index(t => t.BusinessWorkOfficeDetailId);
            
            CreateTable(
                "dbo.MarketeChecks",
                c => new
                    {
                        MarketeCheckId = c.Long(nullable: false),
                        NeighborsName = c.String(),
                        NeighborsAddress = c.String(),
                        KnowsApplicant = c.Boolean(nullable: false),
                        KnowsHowLong = c.String(),
                        NeighborComments = c.String(),
                        BusinessEstablishedSinceMarketeCheck = c.String(),
                        NeighborsTwoName = c.String(),
                        NeighborsTwoAddress = c.String(),
                        NeighborsTwoKnowsApplicant = c.Boolean(nullable: false),
                        NeighborsTwoKnowsHowLong = c.String(),
                        NeighborsTwoNeighborComments = c.String(),
                        NeighborsTwoBusinessEstablishedSinceMarketeCheck = c.String(),
                    })
                .PrimaryKey(t => t.MarketeCheckId)
                .ForeignKey("dbo.WorkOfficeVerifications", t => t.MarketeCheckId)
                .Index(t => t.MarketeCheckId);
            
            CreateTable(
                "dbo.OfficeAddressDetails",
                c => new
                    {
                        OfficeAddressDetailId = c.Long(nullable: false),
                        WasActualAddressSame = c.Boolean(nullable: false),
                        CorrectAddress = c.String(),
                        EstablishedTime = c.String(),
                        WorkAtGivenAddress = c.Boolean(nullable: false),
                        GiveNewAddress = c.String(),
                        IsApplicantAvailable = c.Boolean(nullable: false),
                        MetPersonName = c.String(),
                        GiveReason = c.String(),
                        CNICOS = c.String(),
                        CNICNo = c.String(),
                        MetPersonCNIC = c.String(),
                    })
                .PrimaryKey(t => t.OfficeAddressDetailId)
                .ForeignKey("dbo.WorkOfficeVerifications", t => t.OfficeAddressDetailId)
                .Index(t => t.OfficeAddressDetailId);
            
            CreateTable(
                "dbo.OfficeHRDetails",
                c => new
                    {
                        OfficeHRDetailId = c.Long(nullable: false),
                        NameOfPersonToMeet = c.String(),
                        OHrKnowsApplicant = c.Boolean(nullable: false),
                        ApplicantEmployementStatus = c.String(),
                        ApplicantEmployementPeriod = c.String(),
                        ApplicantDesignation = c.String(),
                        OHrNatureOfBusiness = c.String(),
                        OHrOtherNatureOfBusiness = c.String(),
                        GrossSalary = c.Double(nullable: false),
                        NetTakeHomeSalary = c.Double(nullable: false),
                        SalarySlipVerified = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OfficeHRDetailId)
                .ForeignKey("dbo.WorkOfficeVerifications", t => t.OfficeHRDetailId)
                .Index(t => t.OfficeHRDetailId);
            
            CreateTable(
                "dbo.ApplicationSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ErrorLogs",
                c => new
                    {
                        ErrorLogId = c.Long(nullable: false, identity: true),
                        Error = c.String(),
                        Detail = c.String(),
                        ActionName = c.String(),
                        RecortAt = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ErrorLogId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserProducts",
                c => new
                    {
                        UserProductId = c.Long(nullable: false, identity: true),
                        UserId = c.String(),
                        ProductId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.UserProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.OfficeHRDetails", "OfficeHRDetailId", "dbo.WorkOfficeVerifications");
            DropForeignKey("dbo.OfficeAddressDetails", "OfficeAddressDetailId", "dbo.WorkOfficeVerifications");
            DropForeignKey("dbo.MarketeChecks", "MarketeCheckId", "dbo.WorkOfficeVerifications");
            DropForeignKey("dbo.WorkOfficeVerifications", "InquiryId", "dbo.Inquiries");
            DropForeignKey("dbo.BusinessWorkOfficeDetails", "BusinessWorkOfficeDetailId", "dbo.WorkOfficeVerifications");
            DropForeignKey("dbo.TenantVerifications", "InquiryId", "dbo.Inquiries");
            DropForeignKey("dbo.Status", "InquiryId", "dbo.Inquiries");
            DropForeignKey("dbo.Status", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SalarySlipVerifications", "InquiryId", "dbo.Inquiries");
            DropForeignKey("dbo.ResidenceProfiles", "ResidenceVerificationId", "dbo.ResidenceVerifications");
            DropForeignKey("dbo.ResidenceDetails", "ResidenceDetailId", "dbo.ResidenceVerifications");
            DropForeignKey("dbo.NeighbourChecks", "NeighbourCheckId", "dbo.ResidenceVerifications");
            DropForeignKey("dbo.ResidenceVerifications", "InquiryId", "dbo.Inquiries");
            DropForeignKey("dbo.InquiryImages", "InquiryId", "dbo.Inquiries");
            DropForeignKey("dbo.Inquiries", "CustomerBranchId", "dbo.CustomerBranches");
            DropForeignKey("dbo.Inquiries", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.InquiryApplicationUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.InquiryApplicationUsers", "InquiryId", "dbo.Inquiries");
            DropForeignKey("dbo.AspNetUsers", "CustomerBranchId", "dbo.CustomerBranches");
            DropForeignKey("dbo.CustomerBranches", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Inquiries", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Inquiries", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.AspNetUsers", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.BankStatementVerifications", "InquiryId", "dbo.Inquiries");
            DropForeignKey("dbo.Addresses", "InquiryId", "dbo.Inquiries");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.OfficeHRDetails", new[] { "OfficeHRDetailId" });
            DropIndex("dbo.OfficeAddressDetails", new[] { "OfficeAddressDetailId" });
            DropIndex("dbo.MarketeChecks", new[] { "MarketeCheckId" });
            DropIndex("dbo.BusinessWorkOfficeDetails", new[] { "BusinessWorkOfficeDetailId" });
            DropIndex("dbo.WorkOfficeVerifications", new[] { "InquiryId" });
            DropIndex("dbo.TenantVerifications", new[] { "InquiryId" });
            DropIndex("dbo.Status", new[] { "InquiryId" });
            DropIndex("dbo.Status", new[] { "UserId" });
            DropIndex("dbo.SalarySlipVerifications", new[] { "InquiryId" });
            DropIndex("dbo.ResidenceProfiles", new[] { "ResidenceVerificationId" });
            DropIndex("dbo.ResidenceDetails", new[] { "ResidenceDetailId" });
            DropIndex("dbo.NeighbourChecks", new[] { "NeighbourCheckId" });
            DropIndex("dbo.ResidenceVerifications", new[] { "InquiryId" });
            DropIndex("dbo.InquiryImages", new[] { "InquiryId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.InquiryApplicationUsers", new[] { "UserId" });
            DropIndex("dbo.InquiryApplicationUsers", new[] { "InquiryId" });
            DropIndex("dbo.CustomerBranches", new[] { "CompanyId" });
            DropIndex("dbo.Products", new[] { "CompanyId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "CustomerBranchId" });
            DropIndex("dbo.AspNetUsers", new[] { "BranchId" });
            DropIndex("dbo.AspNetUsers", new[] { "CompanyId" });
            DropIndex("dbo.BankStatementVerifications", new[] { "InquiryId" });
            DropIndex("dbo.Inquiries", new[] { "ProductId" });
            DropIndex("dbo.Inquiries", new[] { "CustomerBranchId" });
            DropIndex("dbo.Inquiries", new[] { "BranchId" });
            DropIndex("dbo.Inquiries", new[] { "CompanyId" });
            DropIndex("dbo.Addresses", new[] { "InquiryId" });
            DropTable("dbo.UserProducts");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ErrorLogs");
            DropTable("dbo.ApplicationSettings");
            DropTable("dbo.OfficeHRDetails");
            DropTable("dbo.OfficeAddressDetails");
            DropTable("dbo.MarketeChecks");
            DropTable("dbo.BusinessWorkOfficeDetails");
            DropTable("dbo.WorkOfficeVerifications");
            DropTable("dbo.TenantVerifications");
            DropTable("dbo.Status");
            DropTable("dbo.SalarySlipVerifications");
            DropTable("dbo.ResidenceProfiles");
            DropTable("dbo.ResidenceDetails");
            DropTable("dbo.NeighbourChecks");
            DropTable("dbo.ResidenceVerifications");
            DropTable("dbo.InquiryImages");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.InquiryApplicationUsers");
            DropTable("dbo.CustomerBranches");
            DropTable("dbo.Products");
            DropTable("dbo.Companies");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Branches");
            DropTable("dbo.BankStatementVerifications");
            DropTable("dbo.Inquiries");
            DropTable("dbo.Addresses");
        }
    }
}
