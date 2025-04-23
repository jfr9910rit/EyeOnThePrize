#include "FastLED.h"

// how many LEDs in your strip?
#define NUM_LEDS 300
#define DATA_PIN 3
CRGB leds[NUM_LEDS];

int incomingByte = 0;
int state = 0; // 0 = idle, 1 = ep, 2 = tb, 3 = hrtly

int fadeClock = 0;

void setup () {
  FastLED.addLeds<WS2812B, DATA_PIN, GRB>(leds, NUM_LEDS);
  FastLED.setBrightness(100);
  Serial.begin(9600);
  delay(2000);
}

void loop() {
  fadeClock++;
  if (fadeClock >= 255) {
    fadeClock = 0;
  }

  switch(state) {
    case 0: // IDLE || RGB fade while waiting for player
      solidColor( CHSV( fadeClock, 100, 100) );
      break;
    // when player active, pulsing solid color corresponding to their character
    case 1: // EP
      characterColorPulse(42);
      //solidColor( CHSV( 85, 255, fadeClock) );
      break;
    case 2: // TB
      characterColorPulse(85);
      //solidColor( CHSV( 85, 255, fadeClock) );
      break;
    case 3: // HRTLY
      characterColorPulse(230);
      //solidColor( CHSV( 230, 255, fadeClock) );
      break;
    default:
      break;
  }

  // check for serial port inputs from unity
  if (Serial.available() > 0) {
    // read the incoming byte:
    incomingByte = Serial.read();

    switch(incomingByte) {
      case 48: // 0 - set to idle
        state = 0;
        break;
      case 49: // 1 - set to ep
        state = 1;
        break;
      case 50: // 2 - set to tb
        state = 2;
        break;
      case 51: // 3 - set to hrtly
        state = 3;
        break;
      case 52: // 4 - incorrect answer
        flashWrong();
        break;
      default:
        break;
    }

    // say what you got:
    //Serial.print("I received: ");
    //Serial.println(incomingByte, DEC);
    //Serial.println(state);
  }

  delay(1);
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
