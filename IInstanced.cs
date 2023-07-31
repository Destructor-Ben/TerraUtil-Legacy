namespace TerraUtil;
public interface IInstanced<TSelf> where TSelf : class
{
    public static virtual TSelf Instance => ModContent.GetInstance<TSelf>();
}