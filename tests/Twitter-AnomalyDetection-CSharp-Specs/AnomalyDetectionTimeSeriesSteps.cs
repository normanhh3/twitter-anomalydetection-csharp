namespace Twitter_AnomalyDetection_CSharp_Specs
{
    using System;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using com.github.ruananswer.anomaly;
    using CsvHelper;
    using CsvHelper.Configuration;
    using LanguageExt;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;
    using static com.github.ruananswer.anomaly.DetectAnoms;

    [Binding]
    public class AnomalyDetectionTimeSeriesSteps
    {
        private Config config = new Config();
        private ImmutableList<TestDataEntry> testData;
        private TryOption<ANOMSResult> result;

        [Given(@"I have an input set of test data")]
        public void GivenIHaveAnInputSetOfTestData()
        {
            using (var reader = new StringReader(Resources.data))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.RegisterClassMap<TestDataEntryMap>();
                this.testData = csv.GetRecords<TestDataEntry>().ToImmutableList();
            }

            Assert.AreEqual(766, this.testData.Count);
        }

        [Given(@"the number of observations per period is (.*)")]
        public void GivenTheNumberOfObservationsPerPeriodIs(int numObservationsPerPeriod)
        {
            this.config.NumObsPerPeriod = numObservationsPerPeriod;
        }

        [Given(@"the first (.*) values are NaN")]
        public void GivenTheFirstValuesAreNaN(int numOfValues)
        {
            for (var i = 0; i < numOfValues - 1; i++)
            {
                this.testData[i].Value = double.NaN;
            }
        }

        [Given(@"the last value is NaN")]
        public void GivenTheLastValueIsNaN()
        {
            this.testData[this.testData.Count - 1].Value = double.NaN;
        }

        [Given(@"the maximum anomalies are (.*)")]
        public void GivenTheMaximumAnomaliesAre(double maxAnomalies)
        {
            this.config.MaxAnoms = maxAnomalies;
        }

        [Given(@"the direction is (.*)")]
        public void GivenTheDirectionIsBoth(string direction)
        {
            // TODO: This is a feature that is not mapped over from the original R implementation!
        }

        [Given(@"the middle value in the data is NaN")]
        public void GivenTheMiddleValueInTheDataIsNaN()
        {
            var middleIndex = (int)Math.Floor((double)this.testData.Count / 2d);
            this.testData[middleIndex].Value = double.NaN;
        }

        [When(@"I request the time-series anomalies for the input data")]
        public void WhenIRequestTheTime_SeriesAnomaliesForTheInputData()
        {
            var timestamps = this.testData.Select(x => x.Date.Ticks).ToArray();
            var values = this.testData.Select(x => x.Value).ToArray();

            var detector = new DetectAnoms(this.config);
            this.result = () => detector.anomalyDetection(timestamps, values);
        }

        [Then(@"I expect there to be (.*) anomalies")]
        public void ThenIExpectThereToBeAnomalies(int numOfAnomalies) =>
            Assert.AreEqual(numOfAnomalies, this.result
                .Match(x => x.AnomsIndex.Length, () => 0, x => 0));

        [Then(@"I expect an exception to be thrown")]
        public void ThenIExpectAnExceptionToBeThrown() =>
            this.result.Match(
                x => Assert.AreEqual(0, x.AnomsIndex.Length),
                () => Assert.Fail("Expected an exception, not no anomalies!"),
                x => Assert.IsInstanceOfType(x, typeof(Exception)));

        // This class is embedded here due to limitations of the ClassMap type visibility combined with the
        // appropriate visibility level of this class.
        private sealed class TestDataEntryMap : ClassMap<TestDataEntry>
        {
            public TestDataEntryMap()
            {
                this.Map(m => m.Date).Name("date");
                this.Map(m => m.Value).Name("value");
            }
        }
    }
}
