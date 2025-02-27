import { useState } from "react";
import PropTypes from "prop-types";

const PencilIcon = ({ fillColor, hoverColor, width }) => {
  const [currentColor, setCurrentColor] = useState(fillColor);

  return (
    <svg
      xmlns="http://www.w3.org/2000/svg"
      viewBox="0 0 50 50"
      width={width}
      height={width}
      onPointerEnter={() => setCurrentColor(hoverColor)}
      onPointerLeave={() => setCurrentColor(fillColor)}
    >
      <path
        fill={currentColor}
        d="M9.6 40.4l2.5-9.9L27 15.6l7.4 7.4-14.9 14.9-9.9 2.5zm4.3-8.9l-1.5 6.1 6.1-1.5L31.6 23 27 18.4 13.9 31.5z"
      />
      <path
        fill={currentColor}
        d="M17.8 37.3c-.6-2.5-2.6-4.5-5.1-5.1l.5-1.9c3.2.8 5.7 3.3 6.5 6.5l-1.9.5z"
      />
      <path
        fill={currentColor}
        d="M29.298 19.287l1.414 1.414-13.01 13.02-1.414-1.412z"
      />
      <path
        fill={currentColor}
        d="M11 39l2.9-.7c-.3-1.1-1.1-1.9-2.2-2.2L11 39z"
      />
      <path
        fill={currentColor}
        d="M35 22.4L27.6 15l3-3 .5.1c3.6.5 6.4 3.3 6.9 6.9l.1.5-3.1 2.9zM30.4 15l4.6 4.6.9-.9c-.5-2.3-2.3-4.1-4.6-4.6l-.9.9z"
      />
    </svg>
  );
};

PencilIcon.defaultProps = {
  fillColor: "black",
  hoverColor: "grey",
  width: "1em",
};

PencilIcon.propTypes = {
  fillColor: PropTypes.string,
  hoverColor: PropTypes.string,
  width: PropTypes.string,
};

export default PencilIcon;
