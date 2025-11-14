import os
import json
from datetime import datetime, timezone

import paho.mqtt.client as mqtt

from app.config import settings
from config.settings import load_config

DATABASE_URL = "postgresql://postgres:password@39.170.11.65:5432/wonz_ai"
import psycopg2 as psycopg

class MQTTIOTClient:
    def __init__(self):
        self.client = mqtt.Client(mqtt.CallbackAPIVersion.VERSION2)
        config = load_config()
        self.client.username_pw_set(
            config.get("mqtt", {}).get("username", ""),
            config.get("mqtt", {}).get("password", ""),
        )
        self.client.on_connect = self.on_connect
        self.client.on_message = self.on_message
        self.mqtt_sub_topic='/devices/+/data'
        self.conn = None

    def _ensure_db(self):
        if self.conn is not None:
            return
        self.conn = psycopg.connect(DATABASE_URL)
        try:
            self.conn.autocommit = True
        except Exception:
            pass
     
    def on_connect(self, client, userdata, flags, rc, properties):
        if rc == 0:
            print("Connected to MQTT Broker")
            client.subscribe(self.mqtt_sub_topic)
            print(f"Subscribed to topic: {self.mqtt_sub_topic}")
        else:
            print(f"Failed to connect, return code {rc}")

    def on_message(self, client, userdata, msg):
        try:
            topic_parts = [p for p in msg.topic.split("/") if p]
            if len(topic_parts) >= 3 and topic_parts[0] == "devices":
                device_id = topic_parts[1]
            elif len(topic_parts) >= 2:
                device_id = topic_parts[1]
            else:
                return
            payload = json.loads(msg.payload.decode())

            temperature = float(payload.get("temperature"))
            humidity = float(payload.get("humidity"))
            pm25 = float(payload.get("pm25"))
            battery = 0
            signal = 0

            ts = payload.get("timestamp_hour") or payload.get("timestamp")
            if ts is None:
                now = datetime.now(timezone.utc)
                timestamp_hour = now.replace(minute=0, second=0, microsecond=0)
            else:
                if isinstance(ts, (int, float)):
                    dt = datetime.fromtimestamp(ts, tz=timezone.utc)
                else:
                    dt = datetime.fromisoformat(str(ts))
                    if dt.tzinfo is None:
                        dt = dt.replace(tzinfo=timezone.utc)
                timestamp_hour = dt.replace(minute=0, second=0, microsecond=0)

            self._ensure_db()
            if self.conn is None:
                print("Postgres client not initialized; set POSTGRES_DSN or related env")
                return

            with self.conn.cursor() as cur:
                cur.execute(
                    "INSERT INTO public.device_metrics (device_id, timestamp_hour, temperature, humidity, pm25, battery, signal) VALUES (%s, %s, %s, %s, %s, %s, %s)",
                    (device_id, timestamp_hour, temperature, humidity, pm25, battery, signal),
                )
            print(f"Inserted metrics for {device_id} at {timestamp_hour.isoformat()}")
        except Exception as e:
            print(f"Error handling metrics message: {e}")

    def connect(self):
        print(f"Connecting to MQTT Broker at {settings.mqtt_endpoint}:{settings.mqtt_port}")
        self.client.connect(settings.mqtt_endpoint, settings.mqtt_port, 60)

    def start(self):
        self.client.loop_start()

    def stop(self):
        self.client.loop_stop()


mqtt_iot = MQTTIOTClient()