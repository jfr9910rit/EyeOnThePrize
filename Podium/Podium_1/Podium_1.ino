
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

void setup() {
  pinMode(redButtonPin, INPUT);
  pinMode(yellowButtonPin, INPUT);
  pinMode(greenButtonPin, INPUT);
  pinMode(blueButtonPin, INPUT);

  Serial.begin(9600);
}

void loop() {
  redButtonState = digitalRead(redButtonPin);
  yellowButtonState = digitalRead(yellowButtonPin);
  blueButtonState = digitalRead(blueButtonPin);
  greenButtonState = digitalRead(greenButtonPin);

  if (redButtonState == HIGH && redDown == false) {
    redDown = true;
    Serial.println("RED");
  }
  if (yellowButtonState == HIGH  && yellowDown == false) {
    yellowDown = true;
    Serial.println("YEL");
  }
  if (blueButtonState == HIGH && blueDown == false) {
    blueDown = true;
    Serial.println("BLU");
  }
  if (greenButtonState == HIGH && greenDown == false) {
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

  delay(50);
}
