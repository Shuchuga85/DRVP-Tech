using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceSchoolApp.Server.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppSetting> AppSettings { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<BlockedPeriod> BlockedPeriods { get; set; }

    public virtual DbSet<ClassValidation> ClassValidations { get; set; }

    public virtual DbSet<Coach> Coaches { get; set; }

    public virtual DbSet<CoachAvailability> CoachAvailabilities { get; set; }

    public virtual DbSet<CoachClass> CoachClasses { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemCategory> ItemCategories { get; set; }

    public virtual DbSet<ItemContact> ItemContacts { get; set; }

    public virtual DbSet<ItemImage> ItemImages { get; set; }

    public virtual DbSet<ItemRequisition> ItemRequisitions { get; set; }

    public virtual DbSet<ItemVariant> ItemVariants { get; set; }

    public virtual DbSet<Modality> Modalities { get; set; }

    public virtual DbSet<NewsPost> NewsPosts { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Parent> Parents { get; set; }

    public virtual DbSet<Participant> Participants { get; set; }

    public virtual DbSet<PersonInfo> PersonInfos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Studio> Studios { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppSetting>(entity =>
        {
            entity.HasKey(e => e.SettingId).HasName("PK__App_Sett__256E1E32D857B6DB");

            entity.ToTable("App_Setting");

            entity.Property(e => e.SettingId).HasColumnName("setting_id");
            entity.Property(e => e.Key)
                .HasMaxLength(64)
                .HasColumnName("key");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Value)
                .HasMaxLength(128)
                .HasColumnName("value");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Audit_Lo__9E2397E076071A8D");

            entity.ToTable("Audit_Log");

            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.Action)
                .HasMaxLength(64)
                .HasColumnName("action");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasColumnName("description");
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.EntityType)
                .HasMaxLength(32)
                .HasColumnName("entity_type");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("ip_address");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK__Audit_Log__id_us__6BE40491");
        });

        modelBuilder.Entity<BlockedPeriod>(entity =>
        {
            entity.HasKey(e => e.BlockedId).HasName("PK__Blocked___BEC6BFD93424BF8A");

            entity.ToTable("Blocked_Period");

            entity.Property(e => e.BlockedId).HasColumnName("blocked_id");
            entity.Property(e => e.EndDatetime).HasColumnName("end_datetime");
            entity.Property(e => e.IdCoach).HasColumnName("id_coach");
            entity.Property(e => e.IdStudio).HasColumnName("id_studio");
            entity.Property(e => e.Reason)
                .HasMaxLength(128)
                .HasColumnName("reason");
            entity.Property(e => e.Scope).HasColumnName("scope");
            entity.Property(e => e.StartDatetime).HasColumnName("start_datetime");

            entity.HasOne(d => d.IdCoachNavigation).WithMany(p => p.BlockedPeriods)
                .HasForeignKey(d => d.IdCoach)
                .HasConstraintName("FK__Blocked_P__id_co__4F47C5E3");

            entity.HasOne(d => d.IdStudioNavigation).WithMany(p => p.BlockedPeriods)
                .HasForeignKey(d => d.IdStudio)
                .HasConstraintName("FK__Blocked_P__id_st__503BEA1C");
        });

        modelBuilder.Entity<ClassValidation>(entity =>
        {
            entity.HasKey(e => e.IdCoachClass).HasName("PK__Class_Va__B764B2D3A716F3CB");

            entity.ToTable("Class_Validation");

            entity.Property(e => e.IdCoachClass)
                .ValueGeneratedNever()
                .HasColumnName("id_coach_class");
            entity.Property(e => e.CoachValAt).HasColumnName("coach_val_at");
            entity.Property(e => e.StaffValAt).HasColumnName("staff_val_at");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.IdCoachClassNavigation).WithOne(p => p.ClassValidation)
                .HasForeignKey<ClassValidation>(d => d.IdCoachClass)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK__Class_Val__id_co__4C6B5938");
        });

        modelBuilder.Entity<Coach>(entity =>
        {
            entity.HasKey(e => e.CoachId).HasName("PK__Coach__2BEBE044AA702FF0");

            entity.ToTable("Coach");

            entity.Property(e => e.CoachId)
                .ValueGeneratedNever()
                .HasColumnName("coach_id");
            entity.Property(e => e.Biography)
                .HasMaxLength(256)
                .HasColumnName("biography");
            entity.Property(e => e.PersonInfoId).HasColumnName("person_info_id");
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(256)
                .HasColumnName("photo_url");
            entity.Property(e => e.IsActive).HasDefaultValue(true).HasColumnName("is_active");


            entity.HasOne(d => d.CoachNavigation).WithOne(p => p.Coach)
                .HasForeignKey<Coach>(d => d.CoachId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Coach__coach_id__2B0A656D");

            entity.HasOne(d => d.PersonInfo).WithMany(p => p.Coaches)
                .HasForeignKey(d => d.PersonInfoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Coach__person_in__2BFE89A6");

            entity.HasMany(d => d.IdModalities).WithMany(p => p.IdCoaches)
                .UsingEntity<Dictionary<string, object>>(
                    "CoachModality",
                    r => r.HasOne<Modality>().WithMany()
                        .HasForeignKey("IdModality")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Coach_Mod__id_mo__3587F3E0"),
                    l => l.HasOne<Coach>().WithMany()
                        .HasForeignKey("IdCoach")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Coach_Mod__id_co__3493CFA7"),
                    j =>
                    {
                        j.HasKey("IdCoach", "IdModality").HasName("PK__Coach_Mo__A09209F2B0819C62");
                        j.ToTable("Coach_Modality");
                        j.IndexerProperty<int>("IdCoach").HasColumnName("id_coach");
                        j.IndexerProperty<int>("IdModality").HasColumnName("id_modality");
                    });
        });

        modelBuilder.Entity<CoachAvailability>(entity =>
        {
            entity.HasKey(e => e.CoachavId).HasName("PK__Coach_Av__FF35DA1BEBDFC01B");

            entity.ToTable("Coach_Availability");

            entity.Property(e => e.CoachavId).HasColumnName("coachav_id");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.IdCoach).HasColumnName("id_coach");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.ValidFrom).HasColumnName("valid_from");
            entity.Property(e => e.ValidUntil).HasColumnName("valid_until");
            entity.Property(e => e.Weekday).HasColumnName("weekday");

            entity.HasOne(d => d.IdCoachNavigation).WithMany(p => p.CoachAvailabilities)
                .HasForeignKey(d => d.IdCoach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Coach_Ava__id_co__3F115E1A");
        });

        modelBuilder.Entity<CoachClass>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Coach_Cl__FDF479863E30D3D1");

            entity.ToTable("Coach_Class");

            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.EndDatetime).HasColumnName("end_datetime");
            entity.Property(e => e.IdCoach).HasColumnName("id_coach");
            entity.Property(e => e.IdModality).HasColumnName("id_modality");
            entity.Property(e => e.IdStudio).HasColumnName("id_studio");
            entity.Property(e => e.MaxParticipants).HasColumnName("max_participants");
            entity.Property(e => e.StartDatetime).HasColumnName("start_datetime");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CoachClasses)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK__Coach_Cla__creat__45BE5BA9");

            entity.HasOne(d => d.IdCoachNavigation).WithMany(p => p.CoachClasses)
                .HasForeignKey(d => d.IdCoach)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK__Coach_Cla__id_co__44CA3770");

            entity.HasOne(d => d.IdModalityNavigation).WithMany(p => p.CoachClasses)
                .HasForeignKey(d => d.IdModality)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK__Coach_Cla__id_mo__42E1EEFE");

            entity.HasOne(d => d.IdStudioNavigation).WithMany(p => p.CoachClasses)
                .HasForeignKey(d => d.IdStudio)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK__Coach_Cla__id_st__43D61337");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Event__2370F727F6E125BB");

            entity.ToTable("Event");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasColumnName("description");
            entity.Property(e => e.EndDatetime).HasColumnName("end_datetime");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(256)
                .HasColumnName("image_url");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.StartDatetime).HasColumnName("start_datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(64)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Item__52020FDD9B06FB0D");

            entity.ToTable("Item");

            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasColumnName("description");
            entity.Property(e => e.FromSchool).HasColumnName("from_school");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdContact).HasColumnName("id_contact");
            entity.Property(e => e.IdOwner).HasColumnName("id_owner");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Items)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FK__Item__id_categor__5CA1C101");

            entity.HasOne(d => d.IdContactNavigation).WithMany(p => p.Items)
                .HasForeignKey(d => d.IdContact)
                .HasConstraintName("FK__Item__id_contact__5D95E53A");

            entity.HasOne(d => d.IdOwnerNavigation).WithMany(p => p.Items)
                .HasForeignKey(d => d.IdOwner)
                .HasConstraintName("FK__Item__id_owner__5BAD9CC8");
        });

        modelBuilder.Entity<ItemCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Item_Cat__D54EE9B422678BE4");

            entity.ToTable("Item_Category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CatgName)
                .HasMaxLength(128)
                .HasColumnName("catg_name");
            entity.Property(e => e.IsActive).HasDefaultValue(true).HasColumnName("is_active");

        });

        modelBuilder.Entity<ItemContact>(entity =>
        {
            entity.HasKey(e => e.IcontactId).HasName("PK__Item_Con__D578A923F0629269");

            entity.ToTable("Item_Contact");

            entity.Property(e => e.IcontactId).HasColumnName("icontact_id");
            entity.Property(e => e.Address)
                .HasMaxLength(128)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .HasColumnName("email");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone_number");
        });

        modelBuilder.Entity<ItemImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__Item_Ima__DC9AC9550C09ADE8");

            entity.ToTable("Item_Images");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.IdItem).HasColumnName("id_item");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(256)
                .HasColumnName("image_url");

            entity.HasOne(d => d.IdItemNavigation).WithMany(p => p.ItemImages)
                .HasForeignKey(d => d.IdItem)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Item_Imag__id_it__634EBE90");
        });

        modelBuilder.Entity<ItemRequisition>(entity =>
        {
            entity.HasKey(e => e.RequisitionId).HasName("PK__Item_Req__2B676C330815AC37");

            entity.ToTable("Item_Requisition");

            entity.Property(e => e.RequisitionId).HasColumnName("requisition_id");
            entity.Property(e => e.ApprovedAt).HasColumnName("approved_at");
            entity.Property(e => e.ExpectedReturnDate).HasColumnName("expected_return_date");
            entity.Property(e => e.IdParent).HasColumnName("id_parent");
            entity.Property(e => e.ItemVariantId).HasColumnName("item_variant_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.RejectedAt).HasColumnName("rejected_at");
            entity.Property(e => e.RequestedAt).HasColumnName("requested_at");
            entity.Property(e => e.ReturnedAt).HasColumnName("returned_at");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.IdParentNavigation).WithMany(p => p.ItemRequisitions)
                .HasForeignKey(d => d.IdParent)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK__Item_Requ__id_pa__690797E6");

            entity.HasOne(d => d.ItemVariant).WithMany(p => p.ItemRequisitions)
                .HasForeignKey(d => d.ItemVariantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK__Item_Requ__item___681373AD");
        });

        modelBuilder.Entity<ItemVariant>(entity =>
        {
            entity.HasKey(e => e.VariantId).HasName("PK__Item_Var__EACC68B70613501F");

            entity.ToTable("Item_Variant");

            entity.Property(e => e.VariantId).HasColumnName("variant_id");
            entity.Property(e => e.Color)
                .HasMaxLength(32)
                .HasColumnName("color");
            entity.Property(e => e.IdItem).HasColumnName("id_item");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Size)
                .HasMaxLength(8)
                .HasColumnName("size");

            entity.HasOne(d => d.IdItemNavigation).WithMany(p => p.ItemVariants)
                .HasForeignKey(d => d.IdItem)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Item_Vari__id_it__607251E5");
        });

        modelBuilder.Entity<Modality>(entity =>
        {
            entity.HasKey(e => e.ModalityId).HasName("PK__Modality__89D7A92F676339C8");

            entity.ToTable("Modality");

            entity.Property(e => e.ModalityId).HasColumnName("modality_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .HasColumnName("name");
            entity.Property(e => e.IsActive).HasDefaultValue(true).HasColumnName("is_active");

        });

        modelBuilder.Entity<NewsPost>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__News_Pos__3ED7876628320502");

            entity.ToTable("News_Post");

            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(256)
                .HasColumnName("image_url");
            entity.Property(e => e.Subtitle)
                .HasMaxLength(128)
                .HasColumnName("subtitle");
            entity.Property(e => e.Title)
                .HasMaxLength(64)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__E059842F52BA75C9");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.EntityType)
                .HasMaxLength(32)
                .HasColumnName("entity_type");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IsSent).HasColumnName("is_sent");
            entity.Property(e => e.Message)
                .HasMaxLength(256)
                .HasColumnName("message");
            entity.Property(e => e.ReadAt).HasColumnName("read_at");
            entity.Property(e => e.Title)
                .HasMaxLength(128)
                .HasColumnName("title");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK__Notificat__id_us__6EC0713C");
        });

        modelBuilder.Entity<Parent>(entity =>
        {
            entity.HasKey(e => e.ParentId).HasName("PK__Parent__F2A60819DD7BFCCE");

            entity.ToTable("Parent");

            entity.Property(e => e.ParentId)
                .ValueGeneratedNever()
                .HasColumnName("parent_id");
            entity.Property(e => e.PersonInfoId).HasColumnName("person_info_id");
            entity.Property(e => e.IsActive).HasDefaultValue(true).HasColumnName("is_active");


            entity.HasOne(d => d.ParentNavigation).WithOne(p => p.Parent)
                .HasForeignKey<Parent>(d => d.ParentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Parent__parent_i__236943A5");

            entity.HasOne(d => d.PersonInfo).WithMany(p => p.Parents)
                .HasForeignKey(d => d.PersonInfoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Parent__person_i__245D67DE");
        });

        modelBuilder.Entity<Participant>(entity =>
        {
            entity.HasKey(e => e.ParticipantId).HasName("PK__Particip__4E037806202033BE");

            entity.ToTable("Participant");

            entity.Property(e => e.ParticipantId).HasColumnName("participant_id");
            entity.Property(e => e.ClassPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("class_price");
            entity.Property(e => e.IdCoachClass).HasColumnName("id_coach_class");
            entity.Property(e => e.IdStudent).HasColumnName("id_student");
            entity.Property(e => e.JoinedAt).HasColumnName("joined_at");
            entity.Property(e => e.ParentValidatedAt).HasColumnName("parent_validated_at");
            entity.Property(e => e.ValidationStatus).HasColumnName("validation_status");

            entity.HasOne(d => d.IdCoachClassNavigation).WithMany(p => p.Participants)
                .HasForeignKey(d => d.IdCoachClass)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK__Participa__id_co__489AC854");

            entity.HasOne(d => d.IdStudentNavigation).WithMany(p => p.Participants)
                .HasForeignKey(d => d.IdStudent)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK__Participa__id_st__498EEC8D");
        });

        modelBuilder.Entity<PersonInfo>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK__Person_I__543848DFEC39E498");

            entity.ToTable("Person_Info");

            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.Address)
                .HasMaxLength(128)
                .HasColumnName("address");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(64)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(64)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__760965CCF99B21C1");

            entity.ToTable("Role");

            entity.HasIndex(e => e.RoleName, "UQ__Role__783254B1E17DC391").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(32)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__1963DD9C32965F32");

            entity.Property(e => e.StaffId)
                .ValueGeneratedNever()
                .HasColumnName("staff_id");
            entity.Property(e => e.PersonInfoId).HasColumnName("person_info_id");
            entity.Property(e => e.Position)
                .HasMaxLength(64)
                .HasColumnName("position");
            entity.Property(e => e.IsActive).HasDefaultValue(true).HasColumnName("is_active");


            entity.HasOne(d => d.PersonInfo).WithMany(p => p.Staff)
                .HasForeignKey(d => d.PersonInfoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Staff__person_in__282DF8C2");

            entity.HasOne(d => d.StaffNavigation).WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Staff__staff_id__2739D489");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__2A33069AE7BEF337");

            entity.ToTable("Student");

            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.IdParent).HasColumnName("id_parent");
            entity.Property(e => e.PersonInfoId).HasColumnName("person_info_id");
            entity.Property(e => e.IsActive).HasDefaultValue(true).HasColumnName("is_active");


            entity.HasOne(d => d.IdParentNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.IdParent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Student__id_pare__2EDAF651");

            entity.HasOne(d => d.PersonInfo).WithMany(p => p.Students)
                .HasForeignKey(d => d.PersonInfoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Student__person___2FCF1A8A");
        });

        modelBuilder.Entity<Studio>(entity =>
        {
            entity.HasKey(e => e.StudioId).HasName("PK__Studio__761C4277367FB07B");

            entity.ToTable("Studio");

            entity.Property(e => e.StudioId).HasColumnName("studio_id");
            entity.Property(e => e.Address)
                .HasMaxLength(256)
                .HasColumnName("address");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .HasColumnName("name");

            entity.HasMany(d => d.IdModalities).WithMany(p => p.IdStudios)
                .UsingEntity<Dictionary<string, object>>(
                    "StudioModality",
                    r => r.HasOne<Modality>().WithMany()
                        .HasForeignKey("IdModality")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Studio_Mo__id_mo__3C34F16F"),
                    l => l.HasOne<Studio>().WithMany()
                        .HasForeignKey("IdStudio")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Studio_Mo__id_st__3B40CD36"),
                    j =>
                    {
                        j.HasKey("IdStudio", "IdModality").HasName("PK__Studio_M__D2B2F716206D87BE");
                        j.ToTable("Studio_Modality");
                        j.IndexerProperty<int>("IdStudio").HasColumnName("id_studio");
                        j.IndexerProperty<int>("IdModality").HasColumnName("id_modality");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__B9BE370F5355CA08");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ__User__F3DBC572DCB225C8").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(256)
                .HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(64)
                .HasColumnName("username");

            entity.HasMany(d => d.IdRoles).WithMany(p => p.IdUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("IdRole")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__User_Role__id_ro__1EA48E88"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__User_Role__id_us__1DB06A4F"),
                    j =>
                    {
                        j.HasKey("IdUser", "IdRole").HasName("PK__User_Rol__1105C276E57275D8");
                        j.ToTable("User_Role");
                        j.IndexerProperty<int>("IdUser").HasColumnName("id_user");
                        j.IndexerProperty<byte>("IdRole").HasColumnName("id_role");
                    });
        });

  
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
