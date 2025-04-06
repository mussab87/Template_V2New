using Microsoft.ReportingServices.OnDemandProcessing.Scalability;
using Microsoft.ReportingServices.ReportIntermediateFormat;
using Microsoft.ReportingServices.ReportIntermediateFormat.Persistence;
using Microsoft.ReportingServices.ReportProcessing;
using System;
using System.Collections.Generic;

namespace Microsoft.ReportingServices.OnDemandProcessing.TablixProcessing
{
	[PersistedWithinRequestOnly]
	internal class RuntimeMapDataRegionObj : RuntimeChartCriObj
	{
		[NonSerialized]
		private static Declaration m_declaration = GetDeclaration();

		internal RuntimeMapDataRegionObj()
		{
		}

		internal RuntimeMapDataRegionObj(IReference<IScope> outerScope, MapDataRegion mapDataRegionDef, ref DataActions dataAction, OnDemandProcessingContext odpContext, bool onePassProcess)
			: base(outerScope, mapDataRegionDef, ref dataAction, odpContext, onePassProcess, Microsoft.ReportingServices.ReportProcessing.ObjectType.MapDataRegion)
		{
		}

		public override void Serialize(IntermediateFormatWriter writer)
		{
			base.Serialize(writer);
			writer.RegisterDeclaration(m_declaration);
			while (writer.NextMember())
			{
				_ = writer.CurrentMember.MemberName;
				Global.Tracer.Assert(condition: false);
			}
		}

		public override void Deserialize(IntermediateFormatReader reader)
		{
			base.Deserialize(reader);
			reader.RegisterDeclaration(m_declaration);
			while (reader.NextMember())
			{
				_ = reader.CurrentMember.MemberName;
				Global.Tracer.Assert(condition: false);
			}
		}

		public override void ResolveReferences(Dictionary<Microsoft.ReportingServices.ReportIntermediateFormat.Persistence.ObjectType, List<MemberReference>> memberReferencesCollection, Dictionary<int, IReferenceable> referenceableItems)
		{
			base.ResolveReferences(memberReferencesCollection, referenceableItems);
		}

		public override Microsoft.ReportingServices.ReportIntermediateFormat.Persistence.ObjectType GetObjectType()
		{
			return Microsoft.ReportingServices.ReportIntermediateFormat.Persistence.ObjectType.RuntimeMapDataRegionObj;
		}

		public new static Declaration GetDeclaration()
		{
			if (m_declaration == null)
			{
				List<MemberInfo> memberInfoList = new List<MemberInfo>();
				return new Declaration(Microsoft.ReportingServices.ReportIntermediateFormat.Persistence.ObjectType.RuntimeMapDataRegionObj, Microsoft.ReportingServices.ReportIntermediateFormat.Persistence.ObjectType.RuntimeChartCriObj, memberInfoList);
			}
			return m_declaration;
		}
	}
}
