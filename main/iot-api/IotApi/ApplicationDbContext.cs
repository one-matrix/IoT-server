using IotApi.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // System tables
    public DbSet<SysUser> SysUsers { get; set; }
    public DbSet<SysUserToken> SysUserTokens { get; set; }
    public DbSet<SysParams> SysParams { get; set; }
    public DbSet<SysDictType> SysDictTypes { get; set; }
    public DbSet<SysDictData> SysDictData { get; set; }

    // AI Model tables
    public DbSet<AiModelProvider> AiModelProviders { get; set; }
    public DbSet<AiModelConfig> AiModelConfigs { get; set; }
    public DbSet<AiTtsVoice> AiTtsVoices { get; set; }

    // Agent tables
    public DbSet<AgentTemplate> AiAgentTemplates { get; set; }
    public DbSet<Agent> AiAgents { get; set; }

    // Device tables
    public DbSet<AiDevice> AiDevices { get; set; }
    public DbSet<AiOta> AiOtas { get; set; }

    // Voiceprint tables
    public DbSet<AgentVoicePrint> AgentVoicePrint { get; set; }
    public DbSet<AiVoiceClone> AiVoiceClones { get; set; }

    // Chat history tables
    public DbSet<AiChatHistory> AiChatHistories { get; set; }
    public DbSet<AiChatMessage> AiChatMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure primary keys
        modelBuilder.Entity<SysUser>().Property(e => e.Id).ValueGeneratedNever();
        modelBuilder.Entity<SysUserToken>().Property(e => e.Id).ValueGeneratedNever();
        modelBuilder.Entity<SysParams>().Property(e => e.Id).ValueGeneratedNever();
        modelBuilder.Entity<SysDictType>().Property(e => e.Id).ValueGeneratedNever();
        modelBuilder.Entity<SysDictData>().Property(e => e.Id).ValueGeneratedNever();

        modelBuilder.Entity<AiModelProvider>().Property(e => e.Id).ValueGeneratedNever();
        modelBuilder.Entity<AiModelConfig>().Property(e => e.Id).ValueGeneratedNever();
        modelBuilder.Entity<AiTtsVoice>().Property(e => e.Id).ValueGeneratedNever();

        modelBuilder.Entity<AgentTemplate>().Property(e => e.Id).ValueGeneratedNever();
        modelBuilder.Entity<Agent>().Property(e => e.Id).ValueGeneratedNever();

        modelBuilder.Entity<AiDevice>().Property(e => e.Id).ValueGeneratedNever();
        modelBuilder.Entity<AiOta>().Property(e => e.Id).ValueGeneratedNever();

        modelBuilder.Entity<AgentVoicePrint>().Property(e => e.Id).ValueGeneratedNever();
        modelBuilder.Entity<AiVoiceClone>().Property(e => e.Id).ValueGeneratedNever();

        modelBuilder.Entity<AiChatHistory>().Property(e => e.Id).ValueGeneratedNever();
        modelBuilder.Entity<AiChatMessage>().Property(e => e.Id).ValueGeneratedNever();

        base.OnModelCreating(modelBuilder);
    }
}