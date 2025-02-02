using System;


namespace asciiadventure {
    public abstract class GameObject {
        
        public int Row {
            get;
            protected set;
        }
        public int Col {
            get;
            protected set;
        }

        public String Token {
            get;
            protected internal set;
        }

        public Screen Screen {
            get;
            protected set;
        }

        public GameObject(int row, int col, String token, Screen screen){
            Row = row;
            Col = col;
            Token = token;
            Screen = screen;
            Screen[row, col] = this;
        }

        public virtual Boolean IsPassable() {
            return false;
        }

        public override String ToString() {
            return this.Token;
        }

        public void Delete() {
            Screen[Row, Col] = null;
        }
    }

    public abstract class MovingGameObject : GameObject {

        public MovingGameObject(int row, int col, String token, Screen screen) : base(row, col, token, screen) {}
        
        public string Move(int deltaRow, int deltaCol) {
            int newRow = deltaRow + Row;
            int newCol = deltaCol + Col;
            if (!Screen.IsInBounds(newRow, newCol)) {
                return "";
            }
            
            GameObject gameObject = Screen[newRow, newCol];
    
            if (gameObject != null && !gameObject.IsPassable()) {
                // TODO: How to handle other objects?
                // walls just stop you
                // objects can be picked up
                // people can be interactd with
                // also, when you move, some things may also move
                // maybe i,j,k,l can attack in different directions?
                // can have a "shout" command, so some objects require shouting
                return "TODO: Handle interaction";
            }
            else if (gameObject != null && gameObject.ToString() == "&" && gameObject.IsPassable()) {
                if(newRow==1 && newCol==6){
                Screen[Row,Col]=null;
                this.Row = 8;
                this.Col = 13;
                }
                else if(newRow==8 && newCol==13){
                Screen[Row,Col]=null;
                this.Row = 1;
                this.Col = 6;
                }
                return "TODO: Handle interaction";
            }
            else if (gameObject != null && gameObject.ToString() == "%" && gameObject.IsPassable()) {
                Game.lives++;
                Game.message = "You just got an extra life";
            }
            else if (gameObject != null && gameObject.ToString() == "!" && gameObject.IsPassable() && this.ToString()!="#" && Game.lives==0) {
                Game.gameOver = true;
                this.Token = "*";
                Game.message = "A trap got you.";
            }
            else if (gameObject != null && gameObject.ToString() == "!" && gameObject.IsPassable() && this.ToString()!="#") {
                Game.lives=0;
                Game.message = "A trap got you, but you got an extra life continue playing";
            }
            // Now just make the move
            int originalRow = Row;
            int originalCol = Col;
            // now change the location of the object, if the move was legal
            Row = newRow;
            Col = newCol;
            if(originalRow==8 && originalCol==13){Teleporter teleporter1 = new Teleporter(8, 13, Screen);}
            else if(originalRow==1 && originalCol==6){Teleporter teleporter1 = new Teleporter(1, 6, Screen);}
            else {Screen[originalRow, originalCol] = null;}
            Screen[Row, Col] = this;
            return "";
        }
    }

    class Wall : GameObject {
        public Wall(int row, int col, Screen screen) : base(row, col, "=", screen) {}
    }

    class Treasure : GameObject {
        public Treasure(int row, int col, Screen screen) : base(row, col, "T", screen) {}

        public override Boolean IsPassable() {
            return true;
        }
    }
    class Teleporter : GameObject {
        public Teleporter(int row, int col, Screen screen) : base(row, col, "&", screen) {}

        public override Boolean IsPassable() {
            return true;
        }
    }

    class Life : GameObject {
        public Life(int row, int col, Screen screen) : base(row, col, "%", screen) {}

        public override Boolean IsPassable() {
            return true;
        }
    }

    class Trap : GameObject {
        public Trap(int row, int col, Screen screen) : base(row, col, "!", screen) {}

        public override Boolean IsPassable() {
            return true;
        }
    }
}