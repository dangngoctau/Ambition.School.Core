using System;
using Ambition.School.Core.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Ambition.School.Core
{
    public class Migrations : DataMigrationImpl
    {

        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(PositionRecord).Name,
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("Name", c => c.WithLength(255))
                    .Column<int>("DisplayOrder")
                );

            SchemaBuilder.CreateTable(typeof(DepartmentRecord).Name,
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("Name", c => c.WithLength(255))
                    .Column<int>("ParentDepartmentRecord_Id", column => column.Nullable())
                );

            SchemaBuilder.CreateTable(typeof(PositionsDepartmentRecord).Name,
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<int>("PositionRecord_Id")
                    .Column<int>("DepartmentRecord_Id")
                );

            SchemaBuilder.CreateTable(typeof(ArticlePermissionPartRecord).Name,
                table => table
                    .ContentPartRecord()
                    .Column<bool>("IsEnabled")
                    .Column<string>("Permissions")
                );

            SchemaBuilder.CreateTable(typeof(ArticlePartRecord).Name,
                table => table
                    .ContentPartRecord()
                );

            SchemaBuilder.CreateTable(typeof(MemberPartRecord).Name,
                table => table
                    .ContentPartRecord()
                    .Column<string>("FirstName")
                    .Column<string>("LastName")
                    .Column<DateTime>("Birthday")
                    .Column<string>("Address")
                    .Column<int>("UserId")
                );

            SchemaBuilder.CreateTable(typeof(TeacherPartRecord).Name,
                table => table
                    .ContentPartRecord()
                    .Column<string>("PositionsDepartments")
                );

            ContentDefinitionManager.AlterTypeDefinition(ContentTypes.Teacher,
                builder => builder
                    .WithPart("CommonPart", p => p
                        .WithSetting("DateEditorSettings.ShowDateEditor", "False")
                        .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "False"))
                    .WithPart("MemberPart")
                    .WithPart("TeacherPart")
                    .Creatable()
                );

            ContentDefinitionManager.AlterTypeDefinition(ContentTypes.Article,
                cfg => cfg
                    .WithPart("CommonPart", p => p
                        .WithSetting("DateEditorSettings.ShowDateEditor", "False")
                        .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "False"))
                    .WithPart("PublishLaterPart")
                    .WithPart("TitlePart")
                    .WithPart("AutoroutePart", builder => builder
                        .WithSetting("AutorouteSettings.AllowCustomPattern", "true")
                        .WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "false")
                        .WithSetting("AutorouteSettings.PatternDefinitions", "[{Name:'Title', Pattern: '{Content.Slug}', Description: 'my-article'}]")
                        .WithSetting("AutorouteSettings.DefaultPatternIndex", "0"))
                    .WithPart("BodyPart")
                    .WithPart("ArticlePermissionPart")
                    .Creatable());

            return 1;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable(typeof(MemberPartRecord).Name, command => command.AddColumn<string>("FullName"));
            return 2;
        }
    }
}