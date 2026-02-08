
import socket
import keyboard


TARGET_PORT = 3501
TARGET_IP = "127.0.0.1"

TARGET_IP = "192.168.178.95"
INDEX = 0

def push_text_to_port(text):
    text.strip()
    text = f"{text}_{INDEX}"
    print(f"Sending: {text}")
    s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    s.sendto(text.encode(), (TARGET_IP, TARGET_PORT))
    s.close()
    

# push_text_to_port("Hello, World!")

# push_text_to_port("b")
# push_text_to_port("a")


key_event_state={}




key_mapping= {
    
    'up': 'y',
    'down': 'a',
    'left': 'x',
    'right': 'b',
    'numpad 0': 'a',
    'numpad 1': 'jlsw',
    'numpad 2': 'jls',
    'numpad 3': 'jlse',
    'numpad 4': 'jlw',
    'numpad 5': 'jln',
    'numpad 6': 'jle',
    'numpad 7': 'jlnw',
    'numpad 8': 'jln',
    'numpad 9': 'jlne',
    'numpad +': 'l1',
    'numpad -': 'r1',
    'numpad *': 'l2',
    'numpad /': 'r2',
    'numpad decimal': 'b',
    'space': 'r1',
    'enter': 'a',
    'esc': 'mr',
    'tab': 'ml',
    
    "attack": "r1",
    "sepcial 0":  "",
    "special 1":  "",
    "special 2":  "",
    "special 3":  "",
    "special 4":  "",
    "special 5":  "",
    "special 6":  "",
    "special 7":  "",
    "special 8":  "",
    "special 9":  "",
    "special 10": "",
    "r1": "r1",
    "d": "ml",
    "f": "mr",
    "a": "a",
    "z": "x",
    "e": "b",
    "r": "y",
    
    
    
    
    
    }

key_mapping_french={
    
    "decimal" :"numpad decimal",
    "0" :"numpad 0",
    "1" :"numpad 1",
    "2" :"numpad 2",
    "3" :"numpad 3",
    "4" :"numpad 4",
    "5" :"numpad 2",
    "6" :"numpad 6",
    "7" :"numpad 7",
    "8" :"numpad 8",
    "9" :"numpad 9",
    "+" :"numpad +",
    "-" :"numpad -",
    "*" :"numpad *",
    "/" :"numpad /",
    "." :"numpad decimal",
    "&" :"1",
    "é" :"2",
    "\"":"3",
    "'" :"4",
    "(" :"5",
    "§" :"6",
    "è" :"7",
    "!" :"8",
    "ç" :"9",
    "à" :"0",
    ")" :")",
    "-" :"-",
    "gauche" :"left",
    "haut" :"up",
    "droite" :"right",
    "bas" :"down",
    "backspace" :"backspace",
    "enter" :"enter",
    "maj" :"shift",
    "ctrl" :"ctrl",
    "alt" :"alt",
    "window gauche": "left windows",
    "window droit": "right windows",
    "alt gr": "alt gr",
    "²": "²",
    "right shift": "right shift",
    "ctrl droite": "right ctrl",
    "<": "sepcial 1",
    ",": "sepcial 2",
    ";": "sepcial 3",
    ":": "sepcial 4",
    "=": "sepcial 5",
    "ù": "sepcial 6",
    "^": "sepcial 7",
    "$": "sepcial 8",
    ")": "sepcial 9",
    "-": "sepcial 0",
    "space": "space",
    "r1": "r1",
    "d": "d",
    "f": "f",
    "a": "a",
    "z": "z",
    "e": "e",
    "r": "r",

        
}


def on_key_event(event):
    global key_event_state
    print(f"{event.name} {event.event_type}")
    
    bool_pressing= event.event_type == keyboard.KEY_DOWN
    bool_releasing= event.event_type == keyboard.KEY_UP
  
    bool_value_changed=False
    current_state= event.event_type
    previous_state= key_event_state.get(event.name, None)
    if current_state != previous_state:
        key_event_state[event.name]= current_state
        bool_value_changed=True
    
    if bool_value_changed:
        if event.name in key_mapping_french:
            en_key = key_mapping_french[event.name]
            if en_key in key_mapping:    
                action = key_mapping[en_key]
                
                
                if bool_pressing:
                    print(f"{event.name} / {en_key} / {action} UP")
                    push_text_to_port(action+"+")
                if bool_releasing:
                    
                    print(f"{event.name} / {en_key} / {action} DOWN")
                    push_text_to_port(action+"-")

keyboard.hook(on_key_event)
keyboard.wait('esc')