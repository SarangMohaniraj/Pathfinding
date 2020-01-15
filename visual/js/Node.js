class Node{
  constructor(boolean walkable, int x, int y, int weight){
    this.walkable=walkable;
    this.x=x;
    this.y=y;
    this.weight=weight; //movement penalty
    this.gCost;
    this.hCost;
  }
  get fCost(){
    return gCost+hCost;
  }
  

}