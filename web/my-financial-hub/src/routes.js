import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import App from './pages/App';

export default function AppRouter() {
  return (
    <Router>
      <Routes>
        <Route path='/' element={App}/>
      </Routes>
    </Router>
  );
}