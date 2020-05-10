﻿using StackOnlyJsonParser.CodeStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackOnlyJsonParser.CodeGeneration
{
	internal static class ArrayGenerator
	{
		public static string Generate(JsonArray array)
		{
			return $@"
using System;
using System.Buffers;
using System.Text.Json;

namespace {array.Namespace}
{{
	{array.Accesibility} readonly ref partial struct {array.TypeName}
	{{
		private readonly Utf8JsonReader _jsonReader;

		public readonly bool HasValue;

		public CollectionEnumerable(ref Utf8JsonReader jsonReader)
		{{
			if (jsonReader.TokenType != JsonTokenType.StartArray && jsonReader.TokenType != JsonTokenType.Null) jsonReader.Read();

			swith (jsonReader.TokenType)
			{{
				case JsonTokenType.StartArray:
					HasValue = true;
					_jsonReader = jsonReader;
					jsonReader.Skip();
					break;

				case JsonTokenType.Null:
					HasValue = false;
					_jsonReader = default;
					break;

				default:
					throw new JsonException($""Expected '[', but got {{jsonReader.TokenType}}"");
			}}
		}}

		public bool Any() => HasValue && _jsonReader.TokenType != JsonTokenType.EndArray;
		public CollectionEnumerator GetEnumerator() => new CollectionEnumerator(_jsonReader);
	}}
}}
";
		}
	}
}