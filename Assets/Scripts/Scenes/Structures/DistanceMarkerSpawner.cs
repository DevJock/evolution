using Keiwando.JSON;
using System.Collections.Generic;
using UnityEngine;

namespace Keiwando.Evolution.Scenes {

    [RegisterInScene(ENCODING_ID)]
    public class DistanceMarkerSpawner: BaseStructure {

        private const string ENCODING_ID = "evolution::structure::distancemarkerspawner";

        public float MarkerDistance { get; private set; }
        public float DistanceAngleFactor { get; private set; }

        public DistanceMarkerSpawner(Transform transform, float markerDistance = 5f, float angleFactor = 1f): base(transform) {
            this.MarkerDistance = markerDistance;
            this.DistanceAngleFactor = angleFactor;
        }

        public override string GetEncodingKey() {
            return ENCODING_ID;
        }

        private static class CodingKey {
            public const string MarkerDistance = "markerDistance";
            public const string AngleFactor = "distanceAngleFactor";
        }

        public override JObject Encode() {
            var json = base.Encode();
            json[CodingKey.MarkerDistance] = MarkerDistance;
            json[CodingKey.AngleFactor] = DistanceAngleFactor;
            return json;
        }

        public static DistanceMarkerSpawner Decode(JObject json) {
            var transform = BaseStructure.DecodeTransform(json);
            var markerDistance = json[CodingKey.MarkerDistance].ToFloat();
            var angleFactor = json[CodingKey.AngleFactor].ToFloat();
            return new DistanceMarkerSpawner(transform, markerDistance, angleFactor);
        }

        public override IStructureBuilder GetBuilder() {
            return new DistanceMarkerSpawnerBuilder(this);
        }

        public class DistanceMarkerSpawnerBuilder: BaseStructureBuilder<DistanceMarkerSpawner> {

            protected override string prefabPath => "Prefabs/Structures/DistanceMarkerSpawner";

            public DistanceMarkerSpawnerBuilder(DistanceMarkerSpawner spawner): base(spawner) {}

            public override GameObject Build(ISceneContext context) {

                if (!context.AreDistanceMarkersEnabled()) return null;

                var spawner = base.Build(context).GetComponent<DistanceMarkerSpawnerBehaviour>();
                spawner.MarkerDistance = this.structure.MarkerDistance;
                spawner.DistanceAngleFactor = this.structure.DistanceAngleFactor;
                spawner.Context = context;
                spawner.gameObject.layer = context.GetBackgroundLayer();
                return spawner.gameObject;
            }
        }
    }
}