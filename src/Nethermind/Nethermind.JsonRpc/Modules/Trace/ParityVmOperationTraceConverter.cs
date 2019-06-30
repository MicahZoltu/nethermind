/*
 * Copyright (c) 2018 Demerzel Solutions Limited
 * This file is part of the Nethermind library.
 *
 * The Nethermind library is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The Nethermind library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with the Nethermind. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Linq;
using Nethermind.Core;
using Nethermind.Core.Extensions;
using Nethermind.Evm.Precompiles;
using Nethermind.Evm.Tracing;
using Newtonsoft.Json;

namespace Nethermind.JsonRpc.Modules.Trace
{
    public class ParityVmOperationTraceConverter : JsonConverter<ParityVmOperationTrace>
    {
        //{
//  "cost": 0.0,
//            "ex": {
//                "mem": null,
//                "push": [],
//                "store": null,
//                "used": 16961.0
//            },
//            "pc": 526.0,
//            "sub": null
//        }
        public override void WriteJson(JsonWriter writer, ParityVmOperationTrace value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WriteProperty("cost", value.Cost);
            writer.WritePropertyName("ex");
            writer.WriteStartObject();
            writer.WritePropertyName("mem");
            if (value.Memory != null)
            {
                for (int i = 0; i < value.Memory.Length; i++)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("data");
                    writer.WriteValue(value.Memory[i].Data.ToHexString(true, false));
                    writer.WritePropertyName("off");
                    writer.WriteValue(value.Memory[i].Offset);
                    writer.WriteEndObject();
                }
            }
            else
            {
                writer.WriteNull();
            }

            writer.WritePropertyName("push");
            if (value.Push != null)
            {
                writer.WriteStartArray();
                for (int i = 0; i < value.Push.Length; i++)
                {
                    writer.WriteValue(value.Push[i].ToHexString(true, true));
                }

                writer.WriteEndArray();
            }
            else
            {
                writer.WriteNull();
            }

            writer.WriteProperty("store", value.Store, serializer);
            writer.WriteProperty("used", value.Used);
            writer.WriteEndObject();

            writer.WriteProperty("pc", value.Pc, serializer);
            writer.WriteProperty("sub", value.Sub, serializer);
            writer.WriteEndObject();
        }

        public override ParityVmOperationTrace ReadJson(JsonReader reader, Type objectType, ParityVmOperationTrace existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}