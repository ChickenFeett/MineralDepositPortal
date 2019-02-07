public class Config
{
	// Dip configuration values
	public int NumberOfRecordsToQueryDip;
	public int DipMarginOfError;
	// Azimuth configuration values
	public int NumberOfRecordsToQueryAzimuth;	
	public int AzimuthMarginOfError;	
	
	
	public Config(int numberOfRecordsToQueryDip, int dipMarginOfError, int numberOfRecordsToQueryAzimuth, int azimuthMarginOfError)
	{
		NumberOfRecordsToQueryDip = numberOfRecordsToQueryDip;
		DipMarginOfError = dipMarginOfError;
		NumberOfRecordsToQueryAzimuth = numberOfRecordsToQueryAzimuth;	
		AzimuthMarginOfError = azimuthMarginOfError;		  
	}								   
	
}
public class DrillLocation
{	
	public Config Config;
	// location of the collar
	public double Latitude;
	public double Longitude;
	// Azimuth and Dip of the collar
	public double CollarAzimuth;
	public double CollarDip;	
	// collection of readings between 0-100
	List<HoleReading> DrillHoleReadings;
	
	
	public DrillLocation(Config config, double latitude, double longitude, double collarAzimuth, double collarDip)
	{
		Config = config;		
		Latitude = latitude;	
		Longitude = longitude;
		CollarAzimuth = collarAzimuth;
		CollarDip = collarDip;
		DrillHoleReadings = new List<HoleReading>();
	}
	
	public void AddReading(HoleReading reading)
	{
		CalculateTrustWorthiness(reading, DrillHoleReadings.Count());
		DrillHoleReadings.Add(reading);
	}
	
	private void CalculateTrustWorthiness(HoleReading reading, int index){
		// retrieve last x records (if x do not exist, take as many as possible up to x)
		var azimuthRecordsToQuery = RetrieveLastXRecords(DrillHoleReadings, index, Config.NumberOfRecordsToQueryAzimuth);
		var dipRecordsToQuery = Config.NumberOfRecordsToQueryAzimuth == Config.NumberOfRecordsToQueryDip 
			? azimuthRecordsToQuery // no need to calculate new list, if they're both going to be the same
			: RetrieveLastXRecords(DrillHoleReadings, index, Config.NumberOfRecordsToQueryDip);
		
		// take averages
		var azimuthAverage = azimuthRecordsToQuery.Sum(x => x.HoleAzimuth) / azimuthRecordsToQuery.Count();		
		var dipAverage = dipRecordsToQuery.Sum(x => x.HoleDip) / dipRecordsToQuery.Count();
		
		// calculate trustworthiness
		reading.TrustWorthiness = Math.Abs(reading.HoleAzimuth - azimuthAverage) < Config.AzimuthMarginOfError &&
								  Math.Abs(reading.HoleDip - dipAverage) < Config.DipMarginOfError
			? 100f
			: 0f;
	}
	
	private void RetrieveStatisticsOfPreviousXHoles(IEnumerable<HoleReading> readings, out float azimuthAverage, out float dipOfPrevious)
	{
		azimuthAverage = readings.Sum(x => x.HoleAzimuth) / readings.Count();
		dipOfPrevious = readings.Sum(x => x.HoleAzimuth) / readings.Count();
	}
	
	private static IEnumerable<HoleReading> RetrieveLastXRecords(List<HoleReading> readings, int startingIndex, int numberOfRecordsToRetrieve)
	{		
		var nRecordsAvailableAfterIndex = Math.Min(startingIndex - readings.Count(), numberOfRecordsToRetrieve);
		return readings.Skip(startingIndex).Take(nRecordsAvailableAfterIndex);
	}
}

public class HoleReading
{
	public float HoleAzimuth;
	public float HoleDip;
	public float TrustWorthiness;
	
	public bool IsTrustworthy => TrustWorthiness - 100.0f < 0.000001f;
}

