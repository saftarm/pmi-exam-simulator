
export default function TestComponent() {

      const user = {
            name: 'John Marston',
            age: 32,
            imageUrl: 'https://i.redd.it/k70ginzzs14b1.jpg',
            imageSize : 200,
      };
      return (
            <div> 
                  <h1>My name is {user.name} and I'm {user.age}</h1>
                  <img src={user.imageUrl}
                  style={{
                        width : user.imageSize,
                        height : user.imageSize
                  }}
                  />
            </div>
      )
}