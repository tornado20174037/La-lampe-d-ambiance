
// By LI Xiangjun
#include <SoftwareSerial.h>
#include <Wire.h>
#include<Adafruit_Sensor.h>
#include"Adafruit_TSL2591.h"
Adafruit_TSL2591 tsl = Adafruit_TSL2591(2591);
void Configuration()
{
  tsl.setTiming(TSL2591_INTEGRATIONTIME_300MS);
  Serial.println("Integration time = 300ms");
  tsl.setGain(TSL2591_GAIN_MED);
  Serial.println(F("25x (medium)"));
}

const int R = 3; const int G = 5; const int B = 6;
int IR; int IB; int IG;
const int MotionSensor = 2;

int redPin = 3;
int greenPin = 5; 
int bluePin = 6; 
int value = 0; 
byte serialData[5]; 
unsigned int red = 20;
unsigned int green = 20;
unsigned int blue = 20;
SoftwareSerial BT(10,11);
char ch[20];
int insize, a;
 int PPC = 1;
 int APP = 1;
void setup() {
  // 此处定义一系列的引脚
  pinMode(redPin, OUTPUT);
  pinMode(greenPin, OUTPUT);
  pinMode(bluePin, OUTPUT);

 //开始串口通信
  Serial.begin(9600);
  BT.begin(9600);
//  insize = BT.available();
//  for(int i=0;i<insize;i++)
//  {
//  ch[i] = BT.read();
//  Serial.print("\n");
//  }
   if(tsl.begin())
  {
    Serial.println("Found a TSL2591 sensor");
  }
  else
  {
    Serial.println("No sensor found");
  }

  Configuration();

  randomSeed(1397);
  Serial.println("Baud Rate: 9600");
 //初始RGB色灯不发光
  setColor(0x00, 0x00, 0x00);
 //定义了一个随机数种子
  randomSeed(1397);
  
}

void setColor(int red, int green, int blue) { 
  analogWrite(redPin, red);
  analogWrite(greenPin,green);
  analogWrite(bluePin, blue);
}
 void setColor_1(int red, int green, int blue) { 
  analogWrite(redPin, red);
  analogWrite(greenPin,green);
  analogWrite(bluePin, blue);

}

void randColor(int num) {

int i,j;
int colorrandom[16581376][3];
for(i=0;i<num;i++){
  for(j=0;j<3;j++){
    colorrandom[i][j]=random(0, 255);
  }
}

for(i=0;i<=num-1;i++){
    setColor_1(colorrandom[i][0],colorrandom[i][1],colorrandom[i][2]);
    delay(500);
    setColor(0, 0, 0);
    delay(200);
}
for(i=num-1;i>=0;i--){
    setColor_1(colorrandom[i][0],colorrandom[i][1],colorrandom[i][2]);
    delay(500);
    setColor(0, 0, 0);
    delay(200);
  }
 
}

void lightFlicker()
{
  int r = serialData[1], g = serialData[2], b = serialData[3];
  
    for (r; r <= 255; r++) {
      setColor(r, g, b);
      delay(4);
    }
    for (g; g <= 255; g++) {
      setColor(r, g, b);
      delay(4);
    }
    for (b; b <= 255; b++) {
      setColor(r, g, b);
      delay(4);
    }
    for (; r >= 0; r--) {
      setColor(r, g, b);
      delay(4);
    }
    for (; g >= 0; g--) {
      setColor(r, g, b);
      delay(4);
    }
    for (; b >= 0; b--) {
      setColor(r, g, b);
      delay(4);
    }
    delay(496);
    setColor(0, 0, 0);
    delay(300);
    setColor(0xff, 0, 0);
    delay(500);
    setColor(0, 0, 0);
    delay(300);
    setColor(0, 0xff, 0);
    delay(500);
    setColor(0, 0, 0);
    delay(300);
    setColor(0, 0, 0xff);
    delay(500);
    setColor(0, 0, 0);
    delay(300);
    randColor(10);
  
}

void color(unsigned int red,unsigned int green,unsigned int blue){ 
  analogWrite(redPin, red);
  analogWrite(bluePin, blue);
  analogWrite(greenPin, green);
}

void randColor_1(int num) {
  while (num--) {
    color(random(0, 255), random(0, 255), random(0, 255));  
    delay(500);   
    color(0, 0, 0);
    delay(200);
  }
}

void lightFlicker_1()
{
  int r = 0, g = 0, b = 0;
  
    for (r; r <= 255; r++) {
      color(r, g, b);
      delay(4);
    }
    for (g; g <= 255; g++) {
      color(r, g, b);
      delay(4);
    }
    for (b; b <= 255; b++) {
      color(r, g, b);
      delay(4);
    }
    for (; r >= 0; r--) {
      color(r, g, b);
      delay(4);
    }
    for (; g >= 0; g--) {
      color(r, g, b);
      delay(4);
    }
    for (; b >= 0; b--) {
      color(r, g, b);
      delay(4);
    }
    delay(496);
    color(0, 0, 0);
    delay(300);
    color(0xff, 0, 0);
    delay(500);
    color(0, 0, 0);
    delay(300);
    color(0, 0xff, 0);
    delay(500);
    color(0, 0, 0);
    delay(300);
    color(0, 0, 0xff);
    delay(500);
    color(0, 0, 0);
    delay(300);
    randColor_1(10);
}

void MotionControl()
{
  //Intensity [0, 255], intensity of led, assigned by app
  //motion sensor controls the led 
  if(digitalRead(MotionSensor)==HIGH)
  {
    //RGB
    //Changement(IR, IG, IB);
    setColor(255,255,255);
    delay(1500);
  }
  else if(digitalRead(MotionSensor)==LOW)
  {
    //Changement(0, 0, 0);
    setColor(0,0,0);
  }
}

uint16_t ReadLumi(void)
{
  //only read visible light
  uint16_t x = tsl.getLuminosity(TSL2591_VISIBLE);
  Serial.print(F("[ ")); 
  Serial.print(millis());
  Serial.print(F(" ms ] "));
  Serial.print(F("Luminosity: "));
  Serial.println(x, DEC);
  return x;
}

void Auto(){
  IR = analogRead(redPin);
  IG = analogRead(greenPin);
  IB = analogRead(bluePin);
  uint16_t lum = ReadLumi();
  if(digitalRead(MotionSensor)==HIGH && lum<400){
    
    setColor(IR, IG, IB);
    delay(15000);
  }
  else{
    setColor(0, 0, 0);
  }
}
void LumControl()
{
 uint16_t Lum = ReadLumi();
//  IR = analogRead(R);
//  IG = analogRead(G);
//  IB = analogRead(B);
  IR = analogRead(redPin);
  IG = analogRead(greenPin);
  IB = analogRead(bluePin);
  if(Lum > 400)
  {
//    analogWrite(R, IR/2);
//    analogWrite(G, IG/2);
//    analogWrite(B, IB/2);
    analogWrite(redPin, 0);
    analogWrite(greenPin,0);
    analogWrite(bluePin, 0);
  }
 else if(Lum<400)
  {
//    analogWrite(R, IR);
//    analogWrite(G, IG);
//    analogWrite(B, IB);
    analogWrite(redPin, IR);
    analogWrite(greenPin, IG);
    analogWrite(bluePin, IB);
  }
}

 
void loop() {
 
  if (Serial.available() > 0) {
    Serial.readBytes(serialData, 5);
    if (serialData[0] == 0x01 && serialData[4] == 0x02) 
    {
      setColor(serialData[1], serialData[2], serialData[3]); 
    }
    if (serialData[0] == 0x02 && serialData[4] == 0x03 )
    {     
         randColor(serialData[3]);
          delay(800);
     }
    if(serialData[0] == 0x01 && serialData[4] == 0x04)
    {
       setColor(serialData[1], serialData[2], serialData[3]);
    }
    if(serialData[0] == 0x01 && serialData[4] == 0x05)
    {
        setColor(0, 0, 0); 
    }
    if( serialData[4] == 0x06)
    {
      for(int i=0; i<= serialData[0]; i++)
      {
        setColor(serialData[1], serialData[2], serialData[3]);
        delay(100);
        setColor(0, 0, 0); 
        delay(100);
      }          
    }
        if(serialData[4] == 0x07)
        {        
        for(int i=0; i<= serialData[0]; i++){
        setColor(serialData[1], serialData[2], serialData[3]);
        delay(500);
        setColor(0, 0, 0); 
        delay(500);
          }
        }
        if(serialData[4] == 0x08)
        { 
        for(int i=0; i<= serialData[0]; i++){
        setColor(serialData[1], serialData[2], serialData[3]);
        delay(1000);
        setColor(0, 0, 0); 
        delay(1000);
          }
        } 
        if(serialData[4] == 0x09 && serialData[0] == 0x01)
        {        
          for(int y=0;y< serialData[2]; y++){
          for (int value = 0 ; value <= 255; value++)
          {
          analogWrite(redPin, value);
          analogWrite(greenPin,0);
          analogWrite(bluePin, 0);
          delay(5);
          }
          for (int value = 255; value >=0; value--)
          {
          analogWrite(redPin, value);
          analogWrite(greenPin,0);
          analogWrite(bluePin, 0);
          delay(5);
          }
           
          delay(800);}
        }
        if(serialData[4] == 0x11 && serialData[0] == 0x01)
        {        
          for(int y=0;y< serialData[2]; y++){
          for (int value = 0 ; value <= 255; value++)
          {
          analogWrite(redPin, 0);
          analogWrite(greenPin,value);
          analogWrite(bluePin, 0);
          delay(5);
          }
          for (int value = 255; value >=0; value--)
          {
          analogWrite(redPin, 0);
          analogWrite(greenPin,value);
          analogWrite(bluePin, 0);
          delay(5);
          }
          delay(800);}
        }
         if(serialData[4] == 0x12 && serialData[0] == 0x01)
        {        
          for(int y=0;y< serialData[2]; y++){
          for (int value = 0 ; value <= 255; value++)
          {
          analogWrite(redPin, 0);
          analogWrite(greenPin,0);
          analogWrite(bluePin, value);
          delay(5);
          }
          for (int value = 255; value >=0; value--)
          {
          analogWrite(redPin, 0);
          analogWrite(greenPin,0);
          analogWrite(bluePin, value);
          delay(5);
          }
          delay(800);}
        }
         if(serialData[4] == 0x10 && serialData[0] == 0x01)
        { 
           lightFlicker();
        }

//         if(serialData[4] == 0x13 && serialData[0] == 0x01)
//        {   
//           //PPC = 1;
//           //LumControl();
//           //MotionControl();
//           Auto();
//        }
//        if(serialData[4] == 0x14 && serialData[0] == 0x01)
//        {   
//           //PPC = 0;
//        }
    }


if(BT.available()>0)
{
    insize = BT.available();
  for(int i=0;i<insize;i++)
  {
  ch[i] = BT.read();
  Serial.print("\n");
  }
if(insize == 1){
  switch(ch[0]){
  case 'a':
    color(150,150,150);
    Serial.println("Command: LED ON");
    delay(5);
  break;
  
  case 'c':
    color(55,55,55);
    Serial.println("Command: LED dim");
    delay(5);
  break;
    
  case 'd':  
    color(100,100,100);
    Serial.println("Command: LED weak");
    delay(5);
  break;

  case 'e':  
    color(150,150,150);
    Serial.println("Command: LED normal");
    delay(5);
  break;

  case 'f':  
    color(200,200,200);
    Serial.println("Command: LED bright");
    delay(5);
  break;

  case 'g':  
    color(255,255,255);
    Serial.println("Command: LED full");
    delay(5);
  break;
    
    
  case 'b':
    color(0,0,0);
    Serial.println("Command: LED OFF");
    delay(5);
  break;
   
   
  case 'R':
    for(int i=0;i<5;i++){
      for(int num=0;num<=255;num+=5)
      {
    color(num,0,0);
    delay(30);
    }
    for(int num=255;num>=0;num-=5)
    {
    color(num,0,0);
    delay(30);
    }
    }
    delay(100);
  break;
 
  case 'G':
    for(int i=0;i<5;i++){
      for(int num=0;num<=255;num+=5)
      {
    color(0,num,0);
    delay(100);
    }
    for(int num=0;num>=0;num-=5)
    {
    color(0,num,0);
    delay(100);
    }
    }
  break;
  
  case'B':
    for(int i=0;i<5;i++){
      for(int num=0;num<=255;num+=5)
      {
       color(0,0,num);
       delay(30);
      }
      for(int num=255;num>=0;num-=5)
      {
       color(0,0,num);
       delay(30);
      }
    }
  break;

  case 'o':
   color(255,0,0);
   Serial.println("Command: LED R");
   delay(5);
  break;

  case 'p':
   color(0,255,0);
   Serial.println("Command: LED G");
   delay(5);
  break;

  case 'q':
   color(0,0,255);
   Serial.println("Command: LED B");
   delay(5);
  break;
    
  case 'h':
   randColor_1(10);
  break;
  
  case 'i':
   lightFlicker_1();
  break;
   }
}
}

//if(PPC == 1){
//LumControl();
//MotionControl();
//}
}





 
