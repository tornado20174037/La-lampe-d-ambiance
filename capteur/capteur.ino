#include <Wire.h>
#include <SoftwareSerial.h>
#include <Adafruit_Sensor.h>
#include "Adafruit_TSL2591.h"

// connect SCL to I2C Clock
// connect SDA to I2C Data
// connect Vin to 3.3-5V DC
// connect GROUND to common ground
Adafruit_TSL2591 tsl = Adafruit_TSL2591(2591);

void Configuration()
{
  //tsl.setGain(TSL2591_GAIN_LOW);    // 1x gain (bright light)
  tsl.setGain(TSL2591_GAIN_MED);      // 25x gain
  //tsl.setGain(TSL2591_GAIN_HIGH);   // 428x gain
  
  //tsl.setTiming(TSL2591_INTEGRATIONTIME_100MS);  // shortest integration time (bright light)
  // tsl.setTiming(TSL2591_INTEGRATIONTIME_200MS);
  tsl.setTiming(TSL2591_INTEGRATIONTIME_300MS);
  // tsl.setTiming(TSL2591_INTEGRATIONTIME_400MS);
  // tsl.setTiming(TSL2591_INTEGRATIONTIME_500MS);
  // tsl.setTiming(TSL2591_INTEGRATIONTIME_600MS);  // longest integration time (dim light)

  /* Display the gain and integration time for reference sake */  
  Serial.println(F("------------------------------------"));
  Serial.print  (F("Gain:         "));
  tsl2591Gain_t gain = tsl.getGain();
  switch(gain)
  {
    case TSL2591_GAIN_LOW:
      Serial.println(F("1x (Low)"));
      break;
    case TSL2591_GAIN_MED:
      Serial.println(F("25x (Medium)"));
      break;
    case TSL2591_GAIN_HIGH:
      Serial.println(F("428x (High)"));
      break;
    case TSL2591_GAIN_MAX:
      Serial.println(F("9876x (Max)"));
      break;
  }
  Serial.print  (F("Timing:       "));
  Serial.print((tsl.getTiming() + 1) * 100, DEC); 
  Serial.println(F(" ms"));
  Serial.println(F("------------------------------------"));
  Serial.println(F(""));
}

const int R = 3; const int G = 5; const int B = 6;
int IR; int IG; int IB;
const int MotionSensor = 2;
int flag = 0;

SoftwareSerial I2CBT(10, 11);

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(MotionSensor, INPUT);
  pinMode(R, OUTPUT); pinMode(G, OUTPUT); pinMode(B, OUTPUT);


  //check sensor
  if (tsl.begin()) 
  {
    Serial.println(F("TSL2591 sensor found"));
  } 
  else 
  {
    Serial.println(F("No sensor found"));
  }
  Configuration();

}

void ChangeColor(int a, int b, int c)
{
  analogWrite(R, a);
  analogWrite(G, b);
  analogWrite(B, c);
}

void Blink(char c)
{
  IR = analogRead(R);
  IG = analogRead(G);
  IB = analogRead(B);
  int gap;
  if(c=='a')
  {
    gap = 100; 
    // 0.1s / 0.5s / 1s ->100, 500, 1000
  }
  
  
  while(1)
  {
    ChangeColor(0, 0, 0);
    delay(gap);
    ChangeColor(IR, IG, IB);
    delay(gap);
  }
}

void RBreath()
{
  int k = 1;
  while(true)
  {
    IR = 255-5*k;
    ChangeColor(IR, 0, 0);
    delay(50);
    k++;
    if(IR==0 || IR==255)
    {
      k=-k;
    }
  }
}

void GBreath()
{
  int k = 1;
  while(true)
  {
    IG = 255-5*k;
    ChangeColor(0, IG, 0);
    delay(50);
    k++;
    if(IG==0 || IG==255)
    {
      k=-k;
    }
  }
}

void BBreath()
{
  int k = 1;
  while(true)
  {
    IB = 255-5*k;
    ChangeColor(0, 0, IB);
    delay(50);
    k++;
    if(IB==0 || IB==255)
    {
      k=-k;
    }
  }
}


uint16_t simpleRead(void)
{
  // Simple data read example. Just read the infrared, fullspecrtrum diode 
  // or 'visible' (difference between the two) channels.
  // This can take 100-600 milliseconds! Uncomment whichever of the following you want to read
  uint16_t x = tsl.getLuminosity(TSL2591_VISIBLE);
  //uint16_t x = tsl.getLuminosity(TSL2591_FULLSPECTRUM);
  //uint16_t x = tsl.getLuminosity(TSL2591_INFRARED);

  Serial.print(F("[ ")); Serial.print(millis()); Serial.print(F(" ms ] "));
  Serial.print(F("Luminosity: "));
  Serial.println(x, DEC);
  return x;
}

void LumControl()
{
  IR = analogRead(R);
  IG = analogRead(G);
  IB = analogRead(B);
  uint16_t lum = simpleRead();
  if(lum<400)
  {
    ChangeColor(IR, IG, IB);
  }
  else if(lum>=400)
  {
    ChangeColor(0, 0, 0);
  }
}

void MotionControl()
{
  IR = analogRead(R);
  IG = analogRead(G);
  IB = analogRead(B);
  Serial.println(MotionSensor);
  if(digitalRead(MotionSensor)==HIGH)
  {
    ChangeColor(IR, IG, IB);
    delay(30000);
  }
  else if(digitalRead(MotionSensor)==LOW)
  {
    ChangeColor(0, 0, 0);
  }
}

void Auto()
{
  IR = analogRead(R);
  IG = analogRead(G);
  IB = analogRead(B);
  uint16_t lum = simpleRead();
  if(digitalRead(MotionSensor)==HIGH && lum<400)
  {
    ChangeColor(IR, IG, IB);
    delay(1500);
  }
  else
  {
    ChangeColor(0, 0, 0);
  }
}

void loop() {
  // put your main code here, to run repeatedly:
  char c;
  if(Serial.available()>0)
  {
    c = Serial.read();
    //Serial.println(c);
    if(c=='a')
    {
      flag = 0;
    }
    else if(c=='l')
    {
      flag = 1;
    }
    else if(c=='m')
    {
      flag = 2;
    }
  }
  if(flag == 0)
  {
    Serial.println("Auto();");
    Auto();
  }
  else if(flag == 1)
  {
    Serial.println("LumControl();");
    LumControl();
  }
  else if(flag == 2)
  {
    Serial.println("MotionControl();");
    MotionControl();
  }
  //LumControl();
  //MotionCOntrol();
}
