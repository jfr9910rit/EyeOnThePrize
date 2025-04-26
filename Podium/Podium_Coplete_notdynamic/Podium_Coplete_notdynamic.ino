#include <Keyboard.h>
#include "FastLED.h"

// BUTTONS
int redButtonPin = 2;
int yellowButtonPin = 3;
int blueButtonPin = 4;
int greenButtonPin = 5;

int redButtonState = 0;
int yellowButtonState = 0;
int blueButtonState = 0;
int greenButtonState = 0;

bool redDown = false;
bool yellowDown = false;
bool blueDown = false;
bool greenDown = false;

// LEDS
// how many LEDs in your strip?
#define NUM_LEDS 86
#define DATA_PIN 6
CRGB leds[NUM_LEDS];

int fadeClock = 0;

void setup() {
  pinMode(redButtonPin, INPUT);
  pinMode(yellowButtonPin, INPUT);
  pinMode(greenButtonPin, INPUT);
  pinMode(blueButtonPin, INPUT);

  FastLED.addLeds<WS2812B, DATA_PIN, GRB>(leds, NUM_LEDS);
  FastLED.setBrightness(100);

  Serial.begin(9600);
  delay(1000);
}

// used by the state switch statement to create a pulsing color effect
void characterColorPulse(int hue) {
  float pulse = sin(fadeClock * 0.0123); // 0.0246 ≈ 2π/255
  uint8_t brightness = (uint8_t)((pulse + 1.0) * 127.5); // scale to 0–255
  solidColor(CHSV(hue, 255, brightness));
}

void solidColor(CHSV color) {
  for (int i = 0; i < NUM_LEDS; i++) {
    leds[i] = color;
  }
  FastLED.show();
}

void flashWrong() {
  for (int i = 0; i < 4; i++) {
    solidColor( CHSV( 0, 255, 255) );
    delay(100);
    solidColor( CHSV( 0, 0, 0) );
    delay(100);
  }
}

void loop() {
  // -- BUTTONS --
  redButtonState = digitalRead(redButtonPin);
  yellowButtonState = digitalRead(yellowButtonPin);
  blueButtonState = digitalRead(blueButtonPin);
  greenButtonState = digitalRead(greenButtonPin);

  if (redButtonState == HIGH && redDown == false) {
    Keyboard.write('v');
    redDown = true;
    Serial.println("RED");
  }
  if (yellowButtonState == HIGH  && yellowDown == false) {
    Keyboard.write('b');
    yellowDown = true;
    Serial.println("YEL");
  }
  if (blueButtonState == HIGH && blueDown == false) {
    Keyboard.write('n');
    blueDown = true;
    Serial.println("BLU");
  }
  if (greenButtonState == HIGH && greenDown == false) {
    Keyboard.write('m');
    greenDown = true;
    Serial.println("GRE");
  }

  if (redButtonState == LOW ) {
    redDown = false;
  }
  if (yellowButtonState == LOW) {
    yellowDown = false;
  }
  if (blueButtonState == LOW) {
    blueDown = false;
  }
  if (greenButtonState == LOW) {
    greenDown = false;
  }

  // -- LEDS --
  fadeClock++;
  if (fadeClock >= 255) {
    fadeClock = 0;
  }

  characterColorPulse(230);
  
  delay(50);
}