import axios from 'axios';

const apiUrl = process.env.REACT_APP_API_URL
axios.defaults.baseURL = apiUrl;

// export const configAxios = () => {

//   axios.interceptors.response.use(
//     function (response) {
//       if (response.data) {
//         // return success
//         if (response.status === 200 || response.status === 201) {
//           return response;
//         }
//         // reject errors & warnings
//         return Promise.reject(response);
//       }

//       // default fallback
//       return Promise.reject(response);
//     },
//     function (error) {
//       // if the server throws an error (404, 500 etc.)
//       return Promise.reject(error);
//     }
//   );
// };



// axios.interceptors.response.use(function (response) {
  
//   return response;
// }, function (error) {
//   console.log("ERROR!");
//   return Promise.reject(error);
// });
export default {
  
  getTasks: async () => {
    const result = await axios.get(apiUrl)    
    return result.data;
  },

  addTask: async(name)=>{
    console.log('addTask', name)
    //TODO
    const result =await axios.post(`${apiUrl}/${name}/false`)  
    return result.data
  },

  setCompleted: async(id, isComplete)=>{
    console.log('setCompleted', {id, isComplete})
    //TODO
    await axios.put(`${apiUrl}/${id} isComplete=${isComplete}`)  
    return {};
  },

  deleteTask:async(id)=>{
    console.log('deleteTask')
    const result = await axios.delete(`${apiUrl}/${id}`)  
    return result.data;
  }
};
