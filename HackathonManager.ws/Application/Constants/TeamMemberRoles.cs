namespace HackathonManager.ws.Application.Constants;

public static class TeamMemberRoles
{
    public const string Leader = "Leader";
    public const string Member = "Member";

    public static string GetRole(int leaderId, int memberId)
        => memberId == leaderId ? Leader : Member;
}
