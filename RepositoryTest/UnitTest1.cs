using RESTcup.Models;

namespace RepositoryTest
{
    public class UnitTest1
    {
        [Fact]
        public void Add_AssignsIdAndAddsToRepository()
        {
            CupsRepository repo = new CupsRepository();
            Cup cup = new Cup { Color = "Yellow", Volume = 150 };

            Cup added = repo.Add(cup);

            Assert.Equal(1, added.Id);
            Cup? fetched = repo.GetById(1);
            Assert.NotNull(fetched);
            Assert.Equal("Yellow", fetched.Color);
            Assert.Equal(150, fetched.Volume);

            IEnumerable<Cup> all = repo.Get();
            Assert.Single(all);
            Assert.Equal(added, all.First());
        }

        [Fact]
        public void Get_ColorAsc_SortsByColorAscending()
        {
            CupsRepository repo = new CupsRepository(includeData: true);

            List<Cup> sorted = repo.Get(sortBy: "colorAsc").ToList();

            string[] expected = new[] { "Blue", "Green", "Red" };
            string[] actual = sorted.Select((Cup c) => c.Color!).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Get_ColorDesc_SortsByColorDescending()
        {
            CupsRepository repo = new CupsRepository(includeData: true);

            List<Cup> sorted = repo.Get(sortBy: "colorDesc").ToList();

            string[] expected = new[] { "Red", "Green", "Blue" };
            string[] actual = sorted.Select((Cup c) => c.Color!).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Get_ColorAlias_SortsByColorAscending()
        {
            CupsRepository repo = new CupsRepository(includeData: true);

            List<Cup> sorted = repo.Get(sortBy: "color").ToList();

            string[] expected = new[] { "Blue", "Green", "Red" };
            string[] actual = sorted.Select((Cup c) => c.Color!).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Get_VolumeAsc_SortsByVolumeAscending()
        {
            CupsRepository repo = new CupsRepository(includeData: true);

            List<Cup> sorted = repo.Get(sortBy: "volumeAsc").ToList();

            int[] expected = new[] { 200, 250, 300 };
            int[] actual = sorted.Select((Cup c) => c.Volume).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Get_VolumeDesc_SortsByVolumeDescending()
        {
            CupsRepository repo = new CupsRepository(includeData: true);

            List<Cup> sorted = repo.Get(sortBy: "volumeDesc").ToList();

            int[] expected = new[] { 300, 250, 200 };
            int[] actual = sorted.Select((Cup c) => c.Volume).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Get_VolumeAlias_SortsByVolumeAscending()
        {
            CupsRepository repo = new CupsRepository(includeData: true);

            List<Cup> sorted = repo.Get(sortBy: "volume").ToList();

            int[] expected = new[] { 200, 250, 300 };
            int[] actual = sorted.Select((Cup c) => c.Volume).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Get_InvalidSortBy_ThrowsArgumentException()
        {
            CupsRepository repo = new CupsRepository(includeData: true);

            Assert.Throws<ArgumentException>(() => repo.Get(sortBy: "invalidSort"));
        }

        [Fact]
        public void Constructor_WithIncludeData_PopulatesRepository()
        {
            CupsRepository repo = new CupsRepository(includeData: true);

            List<Cup> all = repo.Get().ToList();
            Assert.Equal(3, all.Count);

            Assert.Equal(1, all[0].Id);
            Assert.Equal("Red", all[0].Color);

            Assert.Equal(2, all[1].Id);
            Assert.Equal("Blue", all[1].Color);

            Assert.Equal(3, all[2].Id);
            Assert.Equal("Green", all[2].Color);
        }

        [Fact]
        public void Remove_RemovesExisting_ReturnsTrue()
        {
            CupsRepository repo = new CupsRepository(includeData: true);

            bool removed = repo.Delete(2);

            Assert.True(removed);
            Cup? fetched = repo.GetById(2);
            Assert.Null(fetched);
            IEnumerable<Cup> all = repo.Get();
            Assert.Equal(2, all.Count());
            Assert.DoesNotContain(all, (Cup c) => c.Id == 2);
        }

        [Fact]
        public void Remove_NonExisting_ReturnsFalse()
        {
            CupsRepository repo = new CupsRepository();

            bool removed = repo.Delete(999);

            Assert.False(removed);
        }

        [Fact]
        public void Update_Existing_UpdatesProperties_ReturnsTrue()
        {
            CupsRepository repo = new CupsRepository();
            Cup added = repo.Add(new Cup { Color = "Black", Volume = 100 });

            Cup updated = new Cup { Id = added.Id, Color = "White", Volume = 120 };
            bool result = repo.Update(updated);

            Assert.True(result);
            Cup? fetched = repo.GetById(added.Id);
            Assert.NotNull(fetched);
            Assert.Equal("White", fetched.Color);
            Assert.Equal(120, fetched.Volume);
        }

        [Fact]
        public void Update_NonExisting_ReturnsFalse()
        {
            CupsRepository repo = new CupsRepository();

            bool result = repo.Update(new Cup { Id = 42, Color = "Pink", Volume = 50 });

            Assert.False(result);
        }

        [Fact]
        public void GetById_NonExisting_ReturnsNull()
        {
            CupsRepository repo = new CupsRepository();

            Cup? fetched = repo.GetById(12345);

            Assert.Null(fetched);
        }

        // --- New tests for filtering ---

        [Fact]
        public void Get_Filter_ColorContains_IsCaseInsensitiveAndMatchesSubstrings()
        {
            CupsRepository repo = new CupsRepository(includeData: true);

            IEnumerable<Cup> filtered = repo.Get(colorContains: "re");

            // "Red" and "Green" both contain "re" (case-insensitive)
            string[] expected = new[] { "Red", "Green" };
            string[] actual = filtered.Select((Cup c) => c.Color!).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Get_Filter_VolumeAtLeast_IsExclusive_GreaterThan()
        {
            CupsRepository repo = new CupsRepository(includeData: true);

            // volumeAtLeast = 250 -> selects volumes > 250, so only 300
            List<Cup> filtered = repo.Get(minVolume: 250).ToList();

            int[] expected = new[] { 300 };
            int[] actual = filtered.Select((Cup c) => c.Volume).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Get_Filter_ColorAndVolume_WithSorting_AppliesFiltersThenSorts()
        {
            CupsRepository repo = new CupsRepository(includeData: true);

            // colorContains "e" matches Red, Green, Blue; volumeAtLeast 200 selects >200 -> 250 and 300
            List<Cup> result = repo.Get(colorContains: "e", minVolume: 200, sortBy: "volumeDesc").ToList();

            int[] expectedVolumes = new[] { 300, 250 };
            string[] expectedColors = new[] { "Blue", "Red" };
            int[] actualVolumes = result.Select((Cup c) => c.Volume).ToArray();
            string[] actualColors = result.Select((Cup c) => c.Color!).ToArray();

            Assert.Equal(expectedVolumes, actualVolumes);
            Assert.Equal(expectedColors, actualColors);
        }

        [Fact]
        public void Get_Filter_NoMatches_ReturnsEmpty()
        {
            CupsRepository repo = new CupsRepository(includeData: true);

            List<Cup> filtered = repo.Get(colorContains: "purple", minVolume: 1000).ToList();

            Assert.Empty(filtered);
        }
    }
}