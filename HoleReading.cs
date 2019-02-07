public class HoleReading
{
	public float HoleAzimuth;
	public float HoleDip;
	public float TrustWorthiness;
	
	public bool IsTrustworthy => TrustWorthiness - 100.0f < 0.000001f;
}