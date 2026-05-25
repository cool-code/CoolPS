using System;
using System.Collections.Generic;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class UncheckedStringTests
    {
        [Fact]
        public void ImplicitConversion_ToAndFrom_String()
        {
            string s = "Hello, 世界";
            Unchecked.String us = s;
            Assert.NotNull(us);
            Assert.Equal(s.Length, us.Length);
            Assert.Equal(s, (string)us);
            Assert.Equal('H', us[0]);
            Assert.Equal('世', us[7]);
        }

        [Fact]
        public void Enumerator_IteratesChars()
        {
            string s = "Iterate!";
            Unchecked.String us = s;
            var list = new List<char>();
            foreach (char c in us) list.Add(c);
            Assert.Equal(s.ToCharArray(), list.ToArray());
        }

#pragma warning disable CS8600, CS8604
        [Fact]
        public void NullConversions_HandleNull()
        {
            string s = null;
            Unchecked.String us = s;
            Assert.Null(us);
            string back = us;
            Assert.Null(back);
        }
#pragma warning restore CS8600, CS8604

        [Fact]
        public void Indexer_Set_ModifiesString_IntAndUInt()
        {
            var arr = new[] { 'H', 'e', 'l', 'l', 'o' };
            string s = new(arr);
            Unchecked.String us = s;
            // modify via int indexer
            us[1] = 'a';
            // modify via uint indexer
            us[(uint)2] = 'X';
            string back = us;
            Assert.Equal("HaXlo", back);
        }
    }
}
