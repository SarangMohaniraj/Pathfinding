var Heap = require('heap');

//generate grid (canvas) + interactions + Grid class
var c = document.querySelector("#grid");
var ctx = c.getContext("2d");

c.width = window.innerWidth;
c.height = window.innerHeight;

// Binary Heap is much faster than a list

class Grid { 
  constructor(int x, int y, ){
    this.x=x;
    this.y=y;
    this.grid=[[]];
    this.nodeRadius=nodeRadius;
  }
}


generate();
refresh();

// draw + instantiate grid
function generate() {

}

// update changes in selected cubes
function update(){

}

// refresh canvas
function refresh() {
  ctx.clearRect(0, 0, c.width, c.height);
  update();
  render();
  requestAnimationFrame(refresh);
}


//interactions
window.onresize = () => {
  c.width = window.innerWidth;
  c.height = window.innerHeight;
};

c.onmousemove = (event) => {
  if(canJump){
    touchInput = false;
  //mouse position
  let x = event.clientX;
  let y = event.clientY;

  if(typeof pointerX === "number" && typeof pointerY === "number"){
    //difference between initial and current
    let offsetX = x - pointerX;
    let offsetY = y - pointerY;

    velocity.tailX += offsetX/8 * -1;
    velocity.tailY += offsetY/8 * -1;
  }
  //reset
  pointerX = x;
  pointerY = y;
  }
};
c.ontouchmove = (e) => {


  //touch position
  let x = event.touches[0].clientX;
  let y = event.touches[0].clientY;
/*
  if(typeof pointerX === "number" && typeof pointerY === "number"){
    //difference between initial and current
    let offsetX = x - pointerX;
    let offsetY = y - pointerY;

    velocity.tailX += offsetX/8;
    velocity.tailY += offsetY/8;
  }
*/
  //reset
  pointerX = x;
  pointerY = y;

  e.preventDefault();
};
c.ontouchend = () => {
  pointerX = null;
  pointerY = null;
};
document.onmouseleave = () => {
  pointerX = null;
  pointerY = null;
};

c.onmousedown = () => {
  
};
c.onmouseup = () => {
 
};