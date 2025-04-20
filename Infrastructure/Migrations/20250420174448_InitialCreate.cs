using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PictureId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IAs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PictureId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IAs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StripeWebhookEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    PayloadJson = table.Column<string>(type: "jsonb", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StripeWebhookEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    Cpf = table.Column<string>(type: "varchar", maxLength: 14, nullable: false),
                    Phone = table.Column<string>(type: "varchar", maxLength: 20, nullable: false),
                    Endereco = table.Column<string>(type: "varchar", maxLength: 300, nullable: false),
                    Cep = table.Column<string>(type: "varchar", maxLength: 10, nullable: false),
                    Tiktok = table.Column<string>(type: "varchar", maxLength: 255, nullable: true),
                    Instagram = table.Column<string>(type: "varchar", maxLength: 255, nullable: true),
                    GitHub = table.Column<string>(type: "varchar", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "varchar", maxLength: 255, nullable: false),
                    PictureId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    Road = table.Column<string>(type: "varchar", maxLength: 100, nullable: true),
                    NeighborHood = table.Column<string>(type: "varchar", nullable: true),
                    Number = table.Column<long>(type: "bigint", nullable: true),
                    Complement = table.Column<string>(type: "varchar", maxLength: 100, nullable: true),
                    Hash = table.Column<string>(type: "varchar", nullable: true),
                    Salt = table.Column<string>(type: "varchar", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    TokenActivate = table.Column<string>(type: "varchar", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AboutDescription = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalHours = table.Column<decimal>(type: "numeric", nullable: false),
                    NotionUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    GitHubUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IAid = table.Column<Guid>(type: "uuid", nullable: false),
                    TrailerId = table.Column<Guid>(type: "uuid", nullable: true),
                    ParameterId = table.Column<Guid>(type: "uuid", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    PictureId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    Subscribes = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_IAs_IAid",
                        column: x => x.IAid,
                        principalTable: "IAs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PictureId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsFreeStudent = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "varchar", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "varchar", maxLength: 255, nullable: true),
                    FreeCourse = table.Column<bool>(type: "boolean", nullable: true),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parameters_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: true),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: true),
                    IAId = table.Column<Guid>(type: "uuid", nullable: true),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    AwsKey = table.Column<string>(type: "varchar", maxLength: 255, nullable: true),
                    ContentType = table.Column<string>(type: "varchar", nullable: true),
                    UrlExpired = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UrlTemp = table.Column<string>(type: "varchar", maxLength: 255, nullable: true),
                    BucketName = table.Column<string>(type: "varchar", maxLength: 255, nullable: true),
                    TemporaryPath = table.Column<string>(type: "varchar", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pictures_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pictures_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pictures_IAs_IAId",
                        column: x => x.IAId,
                        principalTable: "IAs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pictures_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pictures_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentCourses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentCourses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    PaymentProvider = table.Column<string>(type: "varchar", maxLength: 50, nullable: true),
                    TransactionId = table.Column<string>(type: "varchar", nullable: true),
                    Subscription_EndDate = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    Subscription_StartDate = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    StripeCustomerId = table.Column<string>(type: "varchar", maxLength: 255, nullable: true),
                    StripeSubscriptionId = table.Column<string>(type: "varchar", maxLength: 255, nullable: true),
                    SubscriptionType = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Tempo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    GithubUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    VideoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Views = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lectures_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentLectures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    LectureId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentLectures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentLectures_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentLectures_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentLectures_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LectureId = table.Column<Guid>(type: "uuid", nullable: true),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    AwsKey = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ContentType = table.Column<int>(type: "integer", nullable: true),
                    UrlExpired = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UrlTemp = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BucketName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TemporaryPath = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Videos_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CategoryId",
                table: "Courses",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_IAid",
                table: "Courses",
                column: "IAid");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_ModuleId",
                table: "Lectures",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_CourseId",
                table: "Modules",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_CourseId",
                table: "Parameters",
                column: "CourseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_CategoryId",
                table: "Pictures",
                column: "CategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_CourseId",
                table: "Pictures",
                column: "CourseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_IAId",
                table: "Pictures",
                column: "IAId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_StudentId",
                table: "Pictures",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_TeacherId",
                table: "Pictures",
                column: "TeacherId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_CourseId",
                table: "StudentCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_StudentId",
                table: "StudentCourses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentLectures_CourseId",
                table: "StudentLectures",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentLectures_LectureId",
                table: "StudentLectures",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentLectures_StudentId",
                table: "StudentLectures",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_StudentId",
                table: "Subscriptions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_CourseId",
                table: "Videos",
                column: "CourseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_LectureId",
                table: "Videos",
                column: "LectureId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parameters");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "StripeWebhookEvents");

            migrationBuilder.DropTable(
                name: "StudentCourses");

            migrationBuilder.DropTable(
                name: "StudentLectures");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "IAs");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
