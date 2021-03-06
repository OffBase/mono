//
// SamlConditionsTest.cs
//
// Author:
//	Atsushi Enomoto <atsushi@ximian.com>
//
// Copyright (C) 2006 Novell, Inc.  http://www.novell.com
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Globalization;
using System.IO;
using System.IdentityModel.Claims;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Xml;
using NUnit.Framework;

namespace MonoTests.System.IdentityModel.Tokens
{
	[TestFixture]
	public class SamlConditionsTest
	{
		XmlDictionaryWriter CreateWriter (StringWriter sw)
		{
			return XmlDictionaryWriter.CreateDictionaryWriter (XmlWriter.Create (sw));
		}

		[Test]
		public void DefaultValues ()
		{
			SamlConditions c = new SamlConditions ();
			Assert.AreEqual (DateTime.MinValue.AddDays (1), c.NotBefore, "#1");
			Assert.AreEqual (DateTime.MaxValue.AddDays (-1), c.NotOnOrAfter, "#2");
		}

		[Test]
		public void ConstructorNullConditions ()
		{
			new SamlConditions (DateTime.Now, DateTime.Now.AddMinutes (1), null);
		}

		[Test]
		public void NotBefore ()
		{
			SamlConditions c = new SamlConditions ();
			DateTime min = DateTime.SpecifyKind (DateTime.MinValue, DateTimeKind.Utc);
			c.NotBefore = min;
			Assert.AreEqual (min, c.NotBefore, "#1");
		}

		[Test]
		public void NotOnOrAfter ()
		{
			SamlConditions c = new SamlConditions ();
			DateTime max = DateTime.SpecifyKind (DateTime.MaxValue, DateTimeKind.Utc);
			c.NotOnOrAfter = max;
			Assert.AreEqual (max, c.NotOnOrAfter, "#1");
		}

		[Test]
		public void WriteXml1 ()
		{
			SamlConditions c = new SamlConditions ();
			StringWriter sw = new StringWriter ();
			using (XmlDictionaryWriter dw = CreateWriter (sw)) {
				c.WriteXml (dw, new SamlSerializer (), null);
			}
			Assert.AreEqual (String.Format ("<?xml version=\"1.0\" encoding=\"utf-16\"?><saml:Conditions xmlns:saml=\"{0}\" />", SamlConstants.Namespace), sw.ToString ());
		}

		[Test]
		public void WriteXml2 ()
		{
			SamlConditions c = new SamlConditions ();
			c.Conditions.Add (new SamlDoNotCacheCondition ());
			StringWriter sw = new StringWriter ();
			using (XmlDictionaryWriter dw = CreateWriter (sw)) {
				c.WriteXml (dw, new SamlSerializer (), null);
			}
			Assert.AreEqual (String.Format ("<?xml version=\"1.0\" encoding=\"utf-16\"?><saml:Conditions xmlns:saml=\"{0}\"><saml:DoNotCacheCondition /></saml:Conditions>", SamlConstants.Namespace), sw.ToString ());
		}
	}
}
