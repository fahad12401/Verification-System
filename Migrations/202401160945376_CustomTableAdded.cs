namespace VerificationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomTableAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomAnswers",
                c => new
                    {
                        AnsCode = c.Long(nullable: false, identity: true),
                        Answers = c.String(),
                        QuesCode = c.Long(nullable: false),
                        AnswerType = c.String(),
                    })
                .PrimaryKey(t => t.AnsCode);
            
            CreateTable(
                "dbo.CustomAppAnswers",
                c => new
                    {
                        CustomAppAnsId = c.Long(nullable: false, identity: true),
                        CNIC = c.String(),
                        QuesCode = c.Long(nullable: false),
                        AnsCode = c.Long(nullable: false),
                        Answers = c.String(),
                    })
                .PrimaryKey(t => t.CustomAppAnsId);
            
            CreateTable(
                "dbo.CustomQuestions",
                c => new
                    {
                        QuesCode = c.Long(nullable: false, identity: true),
                        Question = c.String(),
                    })
                .PrimaryKey(t => t.QuesCode);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CustomQuestions");
            DropTable("dbo.CustomAppAnswers");
            DropTable("dbo.CustomAnswers");
        }
    }
}
